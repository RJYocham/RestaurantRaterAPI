using RestaurantRaterAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRaterAPI.Controllers
{
    public class RatingController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> PostRating(Rating model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Restaurant restaurant = await _context.Restaurants.FindAsync(model.RestaurantId);
            if (restaurant == null)
            {
                return BadRequest($"The target restaurant with the Id of {model.RestaurantId} does not exist.");
            }

            _context.Ratings.Add(model);
            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok($"You rated {restaurant.Name} successfully.");
            }

            return InternalServerError();
        }

        //get all ratings?
        [HttpGet]
        public async Task<IHttpActionResult> GetAllRatings()
        {
            List<Rating> ratings = await _context.Ratings.ToListAsync();
            return Ok(ratings);
        }

        //get rating by Id?
        [HttpGet]
        public async Task<IHttpActionResult> GetRatingById(int id)
        {
            Rating rating = await _context.Ratings.FindAsync(id);

            if(rating != null)
            {
                return Ok(rating);
            }
            return NotFound();
        }

        //get rating by restaurant?

        //update rating
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRating(int id, Rating updatedRating)
        {
            if (ModelState.IsValid)
            {
                Rating rating = await _context.Ratings.FindAsync(id);

                if(rating != null)
                {
                    rating.FoodScore = updatedRating.FoodScore;
                    rating.EnvironmentScore = updatedRating.EnvironmentScore;
                    rating.CleanlinessScore = updatedRating.CleanlinessScore;

                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return NotFond();
            }
            return BadRequest(ModelState);
        }

        //delete rating
    }
}
