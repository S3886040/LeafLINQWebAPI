using LeafLINQWebAPI.Model;

namespace LeafLINQWebAPI.DTOs;

public class UserAndPlants
{
    public UserModel user { get; set; }
    public List<Plant> plants { get; set; }
}
