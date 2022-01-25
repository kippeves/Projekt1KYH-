using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Projects;

public class SkillsModel : PageModel
{
    [BindProperty] public string SkillInput { get; set; }
    [BindProperty] public int SkillId { get; set; }
    public SelectList SkillSelect { get; set; }
    private ProjectContext Context;
    public Project ListProject { get; set; }

    public SkillsModel(ProjectContext context)
    {
        Context = context;
    }

    public IActionResult OnGet(int? id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_LoggedIn")))
        {
            return Redirect("/Index");
        }
        ListProject = Context.Projects.Include(p=>p.Skills).SingleOrDefault(p => p.Id == id.Value);
        var consultSkills = ListProject.Skills.ToList();
        var allSkills = Context.Skills.ToList();
        List<Skill> remainingSkills = allSkills.Except(consultSkills).ToList();
        SkillSelect = new SelectList(remainingSkills, "Id", "Name");
        return Page();
    }
}