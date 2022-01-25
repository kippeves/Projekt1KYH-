using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace UI.Pages.Consultants
{
    public class EditModel : PageModel
    {
        private ProjectContext Context;
        [BindProperty] public Consultant ListConsultant { get; set; }
        public string SkillInput { get; set; }
        [BindProperty] public int SkillId { get; set; }
        public SelectList SkillSelect { get; set; }
        
        public EditModel(ProjectContext context)
        {
            Context = context;
        }

        public IActionResult OnGet(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_LoggedIn")))
            {
                return Redirect("/Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            ListConsultant = Context.Consultants.Include(c=>c.Skills).SingleOrDefault(c => c.Id == id.Value);
            var consultSkills = ListConsultant.Skills.ToList();
            var allSkills = Context.Skills.ToList();
            List<Skill> remainingSkills = allSkills.Except(consultSkills).ToList();
            SkillSelect = new SelectList(remainingSkills, "Id", "Name");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Context.Update(ListConsultant);
            Context.SaveChanges();
            return RedirectToPage("/Consultants/Index");
        }

    }
}
