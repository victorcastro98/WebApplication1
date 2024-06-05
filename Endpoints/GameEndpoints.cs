﻿using WebApplication1.Dtos;

namespace WebApplication1.Endpoints;

public static class GameEndpoints
{
const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
 new (
    1,
    "Game 1",
    "Fighting",
    19.99M,
    new DateOnly(1992, 7,15)
 ),
 new (
    2,
    "Game 2",
    "Roleplaying",
    39.99M,
    new DateOnly(2000, 2,1)
 ),
 new (
    3,
    "Game 3",
    "Race",
    28.00M,
    new DateOnly(2010, 12,5)
 ),
 new (
    4,
    "Game 4",
    "Fighting",
    19.99M,
    new DateOnly(2017, 9,6)
 ),
];

public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){

var group = app.MapGroup("games")
.WithParameterValidation();

// GET /games
group.MapGet("/", () => games);

// GET /games/1
group.MapGet("/{id}", (int id) => {
   GameDto? game = games.Find(game => game.Id == id);

   return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName);

// POST /games
group.MapPost("/", (CreateGameDto newGame) => {

    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

// PUT /games
group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>{
   var index = games.FindIndex(game => game.Id == id);

   if(index == -1){
      return Results.NotFound();
   }
   games[index] = new GameDto(
      id,
      updatedGame.Name,
      updatedGame.Genre,
      updatedGame.Price,
      updatedGame.ReleaseDate
   );
   return Results.NoContent();
});

// DELETE games/1
group.MapDelete("/{id}", (int id) =>{
   games.RemoveAll(game => game.Id == id);

   return Results.NoContent();
});

return group;
}
}
