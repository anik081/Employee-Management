using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Controllers
{
	//[Route("api/[controller]")]
	[ApiController]
    public class HomeController : ControllerBase
    {
		[Route("")]
		[HttpGet]
		public IActionResult Index()
		{
			// build response content
			var sb = new StringBuilder();
			sb.Append($@"<html><head><meta charset='utf-8'><title>Routes</title>
				<style>
				p, li {{
					font-family: 'Verdana', sans-serif;
					font-weight: 600;
				}}
				.val {{
						font-family: 'Courier New', Courier, monospace;
						margin:10;
				}}
                table {{
                        border-collapse: collapse;
                }}
                table, th, td {{
                        border: 1px solid black;
                }}
				</style>
				</head><body>");
			sb.Append("Welcome to employee information API. Click <a href=\"https://localhost:44385/Swagger\">here</a> for swagger documentations.");
			sb.Append("</table></body></html>");

			var content = sb.ToString();

			return new ContentResult
			{
				Content = content,
				ContentType = "text/html"
			};
		}
	}
}
