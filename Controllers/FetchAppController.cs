using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FetchWebapp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FetchAppController : ControllerBase
    {
        
        [HttpGet]
        // A simple message in case someone happens to issue a Get() request
        public string Get()
        {
            return "Hey there! Take a look at my documentation to find out how to put me to use.";
        }

        [HttpPost]
        /*
         * Post will be used to make our queries to the web service. 
         * Input format: "email@domain.com, email@domain.com, email@domain.com"
         * Emails should be separated via commas.
         * A valid email includes an @ character to separate the address and its domain. We will naively assume the domain (anything after the '@') is valid
         */
        public int Post([FromBody] string emails)
        {
            // First, split the input by commas and attempt to create a list of valid emails to continue parsing
            List<string> validEmails = new List<string>();

            foreach(string potentialEmail in emails.Split(','))
            {
                // Trim any whitespace from the ends of the string
                string trimmedAddressFull = potentialEmail.Trim();
                // Search for an '@' symbol. If not two sections, 'email' is invalid
                string[] addrDomainSplit = trimmedAddressFull.Split('@');
                if(addrDomainSplit.Length != 2) { continue; }
                // In second element (the domain), verify that there is at least one '.'. From here, we will assume that the user correctly input their domain
                if(!addrDomainSplit[1].Contains('.')) { continue; }
                // In the first element, search for the first/any '+' symbol and ignore everything after it, as Gmail ignores any portion after a username after a '+'.
                string formattedUsername = addrDomainSplit[0].Split('+')[0];
                // Next, remove all '.' characters from the username, as Gmail will ignore the placement of '.' symbols in usernames
                formattedUsername = string.Concat(formattedUsername.Split('.'));

                // Now form the end result of all of this string parsing, increase our tracker, and add it to our list of valid, formatted emails
                string endResult = formattedUsername + "@" + addrDomainSplit[1];

                // Finally, check against the list of valid emails and add our result if it is unique
                if(!validEmails.Contains(endResult))
                {
                    validEmails.Add(endResult);
                }
            }

            return validEmails.Count;
        }
    }
}
