using System.Linq;
using System.Text.Json.Serialization;
using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages;

public class Skills : PageModel
{
    private ProjectContext Context { get; set; }
    
    [BindProperty] public int SkillId { get; set; }
    [BindProperty] public string SkillInput { get; set; }
    [BindProperty ]public string entitytype { get; set; }
    [BindProperty] public Consultant ListConsultant { get; set; }
    [BindProperty] public Project ListProject { get; set; }

    public Skills(ProjectContext context)
    {
        Context = context;
    }
    
    public IActionResult OnGet()
    {
        return Redirect("/Index");
    }

    public IActionResult OnPostSkillSelect()
    {
        Skill selected;
        switch (entitytype)
        {
            case "consultant":
                Consultant dbConsultant = Context.Consultants
                    .Include(c=>c.Skills)
                    .Single(c => c.Id == ListConsultant.Id);
                selected = Context.Skills.Single(s => s.Id == SkillId);
                dbConsultant.Skills.Add(selected);
                Context.Update(dbConsultant);
                Context.SaveChanges();
                return RedirectToPage("/Consultants/Edit",new{id=dbConsultant.Id});
            case "project":
                Project dbProject = Context.Projects
                    .Include(p => p.Skills)
                    .Single(p => p.Id == ListProject.Id);
                selected = Context.Skills.Single(s => s.Id == SkillId);
                dbProject.Skills.Add(selected);
                Context.Update(dbProject);
                Context.SaveChanges();
                return RedirectToPage("/Projects/Skills",new{id=dbProject.Id});
        }
        return RedirectToPage("/Index");
    }

    public IActionResult OnPostSkillInput()
    {
        Skill selected;
        switch (entitytype)
        {
            case "consultant":
                Consultant dbConsultant = Context.Consultants
                    .Include(c=>c.Skills)
                    .Single(c => c.Id == ListConsultant.Id);
                selected = Context.Skills.SingleOrDefault(s=>s.Name==SkillInput);
                if (null == selected)
                {
                    selected = new(){
                        Name = SkillInput
                    };
                }
                dbConsultant.Skills.Add(selected);
                Context.Update(dbConsultant);
                Context.SaveChanges();
                return RedirectToPage("/Consultants/Edit",new{id=dbConsultant.Id});
            case "project":
                Project dbProject = Context.Projects
                    .Include(p => p.Skills)
                    .Single(p => p.Id == ListProject.Id);
                selected = Context.Skills.SingleOrDefault(s=>s.Name==SkillInput);
            {
                selected = new(){
                    Name = SkillInput
                };
            }
                dbProject.Skills.Add(selected);
                Context.Update(dbProject);
                Context.SaveChanges();
                return RedirectToPage("/Projects/Skills",new{id=dbProject.Id});
        }
        return Redirect("/Index");
    }

    public IActionResult OnGetRemove(int? id, int? SkillId, string entitytype)
    {
        switch (entitytype)
        {
            case "consultant":
                Consultant dbConsultant = Context.Consultants
                    .Include(c=>c.Skills)
                    .Single(c => c.Id == id);
                Skill selected = Context.Skills.Single(s => s.Id == SkillId);
                dbConsultant.Skills.Remove(selected);
                Context.Update(dbConsultant);
                Context.SaveChanges();
                return RedirectToPage("/Consultants/Edit",new{ id = dbConsultant.Id});
            case "project":
                Project dbProject = Context.Projects
                    .Include(p => p.Skills)
                    .Single(p => p.Id == id);
                selected = Context.Skills.Single(s => s.Id == SkillId);
                dbProject.Skills.Remove(selected);
                Context.Update(dbProject);
                Context.SaveChanges();
                return RedirectToPage("/Projects/Skills",new{ id = dbProject.Id});
        }
        return Redirect("/Index");
    }
}