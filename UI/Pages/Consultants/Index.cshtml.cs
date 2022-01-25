using DB;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Consultants;

public class Index : PageModel
{
    private ProjectContext Context { get; set; }
    public List<Consultant> IndexList { get; set; }
    public List<Status> StatusList { get; set; }

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
        
        IndexList =  await Context.Consultants
            .AsNoTracking()
            .Include(c => c.Projects)
                .ThenInclude(p=>p.Status)
            .Include(c => c.Skills)
                .ToListAsync();
        return Page();
    }
}