using DB;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Companies;

public class Index : PageModel
{
    private ProjectContext Context { get; set; }
    public List<Company> Content { get; set; }
    
    public Index(ProjectContext context)
    {
        Context = context;
    }
    
    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_LoggedIn")))
        {
            return Redirect("/Index");
        }
        Content =  await Context.Companies
            .AsNoTracking()
            .Include(c => c.Projects)
                .ThenInclude(p=>p.Status)
            .Include(c=>c.Projects)
                .ThenInclude(p=>p.Consultants)
            .ToListAsync();
        return Page();
    }
}