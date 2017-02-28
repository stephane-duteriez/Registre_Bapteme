using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Threading.Tasks;


namespace Bapteme.TagHelpers
{
	public class AlertModalTagHelper : TagHelper
	{
		private const string BodyAttributeName = "asp-body";
		private const string EntityDisplayAttributeName = "asp-entity-name";
		private const string FunctionConfirmantionAttributeName = "asp-confirmation";

		[HtmlAttributeName(BodyAttributeName)]
		public string BodyContent { get; set; }
		[HtmlAttributeName(EntityDisplayAttributeName)]
		public string EntityDisplay { get; set; }
		[HtmlAttributeName(FunctionConfirmantionAttributeName)]
		public string FunctionConfirmation { get; set; }

		public override void Process (TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "div";
			output.Attributes.SetAttribute("class", "modal fade");
			output.Attributes.SetAttribute("tabindex", "-1");
			output.Attributes.SetAttribute("role", "dialog");
			StringBuilder sb = new StringBuilder();
			sb.Append($@"<div class='modal-dialog'>
							<div class='modal-content'>
								<div class='modal-header bg-danger'>
									<button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>
									<h4 class='modal-title'>Suppression d'{EntityDisplay}</h4>
								</div>
							<div class='modal-body'>
								<p>{BodyContent}</p>
							</div>
							<div class='modal-footer'>
								<button type='button' class='btn btn-default' data-dismiss='modal'>Annuler</button>
								<button id='btn-confirm' type='button' class='btn btn-primary' onclick='{FunctionConfirmation}'>Confirmer suppression</button>
							</div>
						</div>
					</div>");
			output.Content.SetHtmlContent(sb.ToString());
		}
	}
}
