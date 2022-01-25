using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Projects;

public class CreateModel : PageModel
{
    private ProjectContext Context { get; set; }
    [BindProperty] public Project Project { get; set; }
    public SelectList StatusSelect { get; set; }
    public SelectList CompanySelect { get; set; }
    public CreateModel(ProjectContext context)
    {
        Context = context;
    }
    
    public IActionResult OnGet()
    {
        CompanySelect = new SelectList(Context.Companies, "Id", "Name");
        StatusSelect = new SelectList(Context.Statuses, "Id", "Name");
        return Page();
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_LoggedIn")))
        {
            return Redirect("/Index");
        }
        if (!ModelState.IsValid)
        {
            return Redirect("/Projects/Create");
        }

        Context.Projects.Add(Project);
        Context.SaveChanges();
        return Redirect("/Projects/Index");
    }
}