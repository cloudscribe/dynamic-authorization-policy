using System.ComponentModel.DataAnnotations;

namespace cloudscribe.DynamicPolicy.Web.Mvc.ViewModels
{
    public class NewPolicyViewModel
    {
        [Required(ErrorMessage = "Name is required, you will be able to edit other policy settings after it is created.")]
        public string Name { get; set; }
    }
}
