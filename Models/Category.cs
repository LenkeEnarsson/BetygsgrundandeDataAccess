using Seido.Utilities.SeedGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Category : ICategory, ISeed<Category>
    {
        public virtual Guid CategoryId { get; set; }
        public string Name { get; set; }

        public virtual List<IAttraction> Attractions { get; set; } = [];



        #region Seeding
        public bool Seeded { get; set; }
        public Category Seed(SeedGenerator seeder) //called by Attractions.cs, Attractions set by caller.
        {
            Seeded = true;
            CategoryId = Guid.NewGuid();
            Name = seeder.FromString("Accommodation, Activity, Adventure Sports, Antique Shop, Aquarium, Architectural Landmark, Art Gallery, Art Museum, Artisan Market, Bakery, Bar, Beach, Bed & Breakfast, Botanical Garden, Boutique Hotel, Brewery, Bridge, Campsite, Castle, Cave, Cavern, City Park, Coastline, Concert Hall, Cultural Center, Dessert Shop, Distillery, Escape Room, Flea Market, Food Hall, Food Market, Fortress, Gaming Center, Garden, Guided Tour, Heritage Center, Hiking Trail, Historical Museum, Historic Site, Hostel, Hot Spring, Hotel, Ice Cream Parlor, Lake, Local Market, Memorial, Monument, Mosque, Museum, National Park, Nature Reserve, Night Market, Observation Deck, Opera House, Palace, Performing Arts Venue, Plaza, Pub, Religious Site, Resort, Restaurant, River, RV Park, Science Museum, Sculpture Park, Shopping Mall, Shrine, Ski Resort, Souvenir Shop, Spa, Speakeasy, Specialized Museum, Sports Stadium, Square, State Park, Statue, Temple, Theater, Theme Park, Tour Operator, Trekking Trail, Viewpoint, Water Park, Water Sports, Waterfall, Wellness Center, Wildlife Sanctuary, Winery, Winter Sports, Zoo");

            return this;
        }
        #endregion
    }
}
