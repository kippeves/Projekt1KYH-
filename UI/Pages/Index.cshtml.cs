using System.Security.Cryptography;
using System.Text;
using DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages;

public class IndexModel : PageModel
{
    private readonly ProjectContext Context;
    public const string SessionKeySecret = "_LoggedIn";
    [BindProperty] public List<Status> IndexList { get; set; }
    [BindProperty] public string PassString { get; set; }
    public Dictionary<string,Dictionary<char, List<Project>>>titleDictionary { get; set; }
    public IndexModel(ProjectContext context)
    {
        Context = context;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        titleDictionary = new();
        IndexList = await Context.Statuses
            .AsNoTracking()
            .Include(s=>s.Projects)
                .ThenInclude(p=>p.Company)
            .Include(s=>s.Projects)
                .ThenInclude(p=>p.Consultants)
            .Include(s=>s.Projects)
                .ThenInclude(p=>p.Skills)
            .Where(s=>s.Name == "To Do" || s.Name == "Doing")
            .ToListAsync();
        foreach (var s in IndexList)
        {
            foreach (var p in s.Projects)
            {
                if (!titleDictionary.Keys.Contains(s.Name))
                {
                    titleDictionary[s.Name] = new();
                    if (!titleDictionary[s.Name].Keys.Contains(p.Name.ToLower()[0]))
                    {
                        titleDictionary[s.Name][p.Name.ToLower()[0]] = new();
                        titleDictionary[s.Name][p.Name.ToLower()[0]].Add(p);
                    }
                    else titleDictionary[s.Name][p.Name.ToLower()[0]].Add(p);
                }
                else
                {
                    if (!titleDictionary[s.Name].Keys.Contains(p.Name.ToLower()[0]))
                    {
                        titleDictionary[s.Name][p.Name.ToLower()[0]] = new();
                        titleDictionary[s.Name][p.Name.ToLower()[0]].Add(p);
                    }
                    else titleDictionary[s.Name][p.Name.ToLower()[0]].Add(p);
                }
            }
        }
        return Page();
    }

    public IActionResult OnGetSaveData()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_LoggedIn")))
        {
            return Redirect("/Index");
        }
        var content = Context.Companies
            .Include(c => c.Projects)
            .Include(c => c.Projects)
                .ThenInclude(p => p.Consultants)
            .Include(p => p.Projects)
                .ThenInclude(p => p.Skills);
        string name = "text.txt";
        FileInfo info = new FileInfo(name);
        using (StreamWriter sb = info.CreateText())
        {
            foreach (Company c in content)
            {
                sb.WriteLine("Company: " + c.Name);
                foreach (Project p in c.Projects)
                {
                    sb.WriteLine("Project: " + p.Name);
                    sb.WriteLine("Requested Skills:");
                    foreach (Skill s in p.Skills)
                    {
                        sb.WriteLine("\t" + s.Name);
                    }
                    sb.WriteLine("Consultants in project:");
                    for(int i = 0; i < p.Consultants.Count; i++)
                    {  
                        sb.WriteLine("\t" + (i+1) + ". "+ p.Consultants.ToList()[i].FullName);
                    }
                    sb.WriteLine();
                }
            }
        }
        return File(info.OpenRead(), "application/octet-stream", name);
    }
    public IActionResult OnPost()
    {
        string? passData = Request.Form["pass"].ToString();
        if (string.IsNullOrEmpty(passData)) return Redirect("/Index");
        var sb = new StringBuilder();
        var enc = Encoding.UTF8;
        using (var hash = SHA256.Create())
        {
            var result = hash.ComputeHash(enc.GetBytes(passData));
            foreach (var b in result)
                sb.Append(b.ToString("x2"));
        }
        if (sb.Equals("4813494d137e1631bba301d5acab6e7bb7aa74ce1185d456565ef51d737677b2".AsSpan()))
        {
            HttpContext.Session.SetString(SessionKeySecret, "yes");
        }
        return Redirect("/Index");
    }
}

