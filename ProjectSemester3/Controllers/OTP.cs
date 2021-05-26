using Microsoft.AspNetCore.Mvc;
using ProjectSemester.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSemester3.Controllers
{
    [Route("otp")]
    public class OTP : Controller
    {
        [HttpPost]
        [Route("numericOTP")]
        public IActionResult GenerateNumericOTP(int length = 6)
        {
            Generator generator = new Generator();

            return Ok(generator.GenerateNumericString(length));
        }

        [HttpPost]
        [Route("guid")]
        public IActionResult GenerateGUID()
        {
            return Ok(Guid.NewGuid().ToString());
        }
    }
}
