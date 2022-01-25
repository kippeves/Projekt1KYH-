using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Company = DB.Models.Company;

namespace UI.Pages.Companies;

public class Create : PageModel
{
    private ProjectContext Context { get; set; }
    [BindProperty] public Company Company { get; set; }

    public Create(ProjectContext context)
    {
        Context = context;
    }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_LoggedIn")))
        {
            return Redirect("/Index");
        } 
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        if (null != Company) { 
            Context.Companies.Add(Company);
            Context.SaveChanges();
        }
        return Redirect("/Companies");
    }
}