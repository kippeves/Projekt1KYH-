using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Consultants;

public class Create : PageModel
{
    private ProjectContext Context { get; set; }
    [BindProperty] public Consultant Consult { get; set; }
    public Create(ProjectContext context)
    {
        Context = context;
    }
    
    public IActionResult OnGet()
    {
        
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
            ViewData["ErrorMessage"] = "You missed filling in some details. Please try again";
            return Page();
        }
        Context.Consultants.Add(Consult);
        Context.SaveChanges();
        return Redirect("/Consultants/Index");
    }
}