using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Consultants
{
    public class ConnectModel : PageModel
    {
        private ProjectContext Context { get; set; }
        [BindProperty]public Project PageProject { get; set; }
        public List<Consultant> ToRemove { get; set; }
        public List<Consultant> ToAdd { get; set; }
        public SelectList AddSelect { get; set; }
        public SelectList RemoveSelect { get; set; }
        [BindProperty] public Consultant ListConsult { get; set; }
        public ConnectModel(ProjectContext context)
        {
            Context = context;
        }
        public IActionResult OnGet(int? id)
        {
            if (!id.HasValue) {
                return Redirect("/Companies/Index");
            }
            PageProject = Context.Projects
                .Include(p => p.Consultants).Single(p=> p.Id == id);
            var projectConsultants = PageProject.Consultants.ToList();
            var allConsultants = Context.Consultants.ToList();
            ToAdd = allConsultants.Except(projectConsultants).ToList();
            RemoveSelect = new SelectList(projectConsultants, "Id", "FullName");
            AddSelect = new SelectList(ToAdd, "Id", "FullName");
            return Page();
        }

        public IActionResult OnPostAdd()
        {
            Project dbProject = Context.Projects.Single(p => p.Id == PageProject.Id);
            Consultant c = Context.Consultants.Single(c => c.Id == ListConsult.Id);
            Context.Entry(dbProject).Collection(c => c.Consultants).Load();
            dbProject.Consultants.Add(c);
            Context.Update(dbProject);
            Context.SaveChanges();
            return RedirectToPage("/Consultants/Connect", new{id=@PageProject.Id});
        }
        
        public IActionResult OnPostRemove()
        {
            PageProject = Context.Projects.Single(p => p.Id == PageProject.Id);
            ListConsult = Context.Consultants.Single(c => c.Id == ListConsult.Id);
            Context.Entry(PageProject).Collection(c => c.Consultants).Load();
            PageProject.Consultants.Remove(ListConsult);
            Context.Update(PageProject);
            Context.SaveChanges();
            return RedirectToPage("/Consultants/Connect", new{id=@PageProject.Id});
        }
    }
}
