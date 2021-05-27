using Microsoft.AspNetCore.Mvc;
using ProjectSemester.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSemester3.Controllers
{
    [Route("otp")]
    public class OTP : Controller
    {
        [HttpGet]
        [Route("numericOTP")]
        public IActionResult GenerateNumericOTP()
        {
            Generator generator = new Generator();
            ViewBag.otp = generator.GenerateOtp();
            Debug.WriteLine("Otp: " + generator.GenerateOtp());
            return View("Index", generator.GenerateOtp());
        }
        /*[HttpPost]
        [Route("numericOTP")]
        public IActionResult GenerateNumericOTP(int length = 6)
        {
            Generator generator = new Generator();
            ViewBag.otp = generator.GenerateNumericString(length);
            Debug.WriteLine("Otp: " + generator.GenerateNumericString(length));
            return View("Index",generator.GenerateNumericString(length));
        }*/

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
