using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpPost("addRecipe")]
        public IActionResult addRecipe(Recipe newRecipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newRecipe);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet("edit/{recipeID}")]
        public IActionResult Edit(int recipeID)
        {
            Recipe oneRecipe = _context.Recipe.FirstOrDefault(rec => rec.RecipeId == recipeID);
            return View(oneRecipe);
        }

        [HttpPost("updateRecipe/{recipeID}")]
        public IActionResult Update(int recipeID, Recipe recipeToUpdate)
        {
            if (ModelState.IsValid)
            {
                // Find the old version
                Recipe oldRecipe = _context.Recipe.FirstOrDefault(rec => rec.RecipeId == recipeID);
                oldRecipe.Name = recipeToUpdate.Name;
                oldRecipe.Chef = recipeToUpdate.Chef;
                oldRecipe.Tastiness = recipeToUpdate.Tastiness;
                oldRecipe.Calories = recipeToUpdate.Calories;
                oldRecipe.Description = recipeToUpdate.Description;
                oldRecipe.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("Edit", recipeToUpdate);
            }
        }

        [HttpGet("delete/{recipeID}")]
        public IActionResult DeleteRecipe(int recipeID)
        {
            // Like Update, we will need to query for a single user from our Context object
            Recipe recipeToDelete = _context.Recipe
                .SingleOrDefault(rec => rec.RecipeId == recipeID);

            // Then pass the object we queried for to .Remove() on Users
            _context.Recipe.Remove(recipeToDelete);

            // Finally, .SaveChanges() will remove the corresponding row representing this User from DB 
            _context.SaveChanges();
            // Other code
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.AllRecipes = _context.Recipe.ToList().OrderBy(rec => rec.Name);
            List<Recipe> AllRecipes = _context.Recipe.ToList();

            return View();
        }
    }
}