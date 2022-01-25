using DB;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Projects;

public class IndexModel : PageModel
{
    private ProjectContext Context { get; set; }
    public List<Project> IndexList { get; set; }

    public IndexModel(ProjectContext context)
    {
        Context = context;
    }
    
    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_LoggedIn")))
        {
            return Redirect("/Index");
        }
        
        IndexList = await Context.Projects
            .AsNoTracking()
            .Include(p => p.Skills)
            .Include(p => p.Company)
            .Include(p => p.Status)
            .Include(p => p.Consultants)
            .ThenInclude(c=>c.Skills)
            .Where(p => p.Status.Name == "To Do" || p.Status.Name == "Doing")
            .OrderBy(p=>p.Name)
            .ToListAsync();
        return Page();
    }
}