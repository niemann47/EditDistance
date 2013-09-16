using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EditDistance.Models;

namespace EditDistance.Controllers
{
    public class LevenshteinController : Controller
    {
        //
        // GET: /Levenshtein/

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult EditDistance(string tags, string search)
        {
            //add in delim characters
            char[] delim = { ',', '.', ' ', ';' };
            //get rid of white space if there is any
            tags.Replace(" ", string.Empty);
            search.Replace(" ", string.Empty);
            //split the tags with the defined delimiters
            string[] seperated = tags.Split(delim);

            tag[] terms = new tag[seperated.Length];

            //loop through and compute the Levenshtein distance with each tag
            for (int i = 0; i < seperated.Length; i++)
            {
                terms[i] = new tag();
                terms[i].Name = seperated[i];
                terms[i].MinDistance = ComputeLevenshtein(search, seperated[i]);
            }

            //sort the items from least to greatest
            Sort(terms);

            //pass the terms to the view to display
            ViewBag.result = terms;
            return PartialView("SearchResults");

        }

        //computeLevenshtein distance
        //taken from wikipedia psudocode
        //using dynamic programming for efficiency
        public int ComputeLevenshtein(string a, string b)
        {
            int[,] values = new int[a.Length+1,b.Length+1];

            //prime with values just in case b is empty
            for(int i = 1; i<a.Length+1; i++)
                values[i,0] = i;

            //prime with values just in case a is empty
            for(int j = 1; j<b.Length+1; j++)
                values[0,j] = j;

            //loop through and fill in structure with distance values
            for (int j = 1; j < b.Length+1; j++)
            {
                for (int i = 1; i < a.Length+1; i++)
                {
                    //check to see if the values are equal (no distance)
                    if (a[i - 1] == b[j - 1])
                        values[i, j] = values[i - 1, j - 1];
                    else 
                    {
                        //get the minimum value for a deletion, an insertion, and a substitution
                        values[i, j] = Math.Min(Math.Min(values[i - 1, j] + 1, values[i, j - 1] + 1), values[i - 1, j - 1] + 1);
                    }
                }
            }

            return values[a.Length,b.Length];
        }

        //simple bubble sort: innefficient
        public void Sort(tag[] terms)
        {
            tag temp = new tag();

            for(int i=0; i<terms.Length; i++)
                for(int j=i+1; j<terms.Length; j++)
                {
                    if(terms[i].MinDistance > terms[j].MinDistance)
                    {
                        temp = terms[i];
                        terms[i] = terms[j];
                        terms[j] = temp;
                    }
                }
        }

    }
}
