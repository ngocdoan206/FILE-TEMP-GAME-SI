Space-Invaders

C# console game based on classic Space Invaders
Usually in every game, the loop looks like this:

For each frame:
  Handle user input
  Update the game's state
  Render the game's state


GameState class contains all the data about the game: Game Score, Player Name, Location of Hero, Bullets, Invaders.

All the information of the current state of the game is stored in this class, then the state is rendered to the UI by this:

void Render(GameState state)


Render will put one frame on the screen, and the game state is one frame.
It’s like screen = view and game state = model, in architectural terms.

HandleFrame Method will update the GameState after each frame.
HandleFrame works like this:

GameState HandleFrame(GameState oldState)
{
    // Handle user input
    // Update locations of all game objects
    // Update Score and escaped invaders
    return oldState;
}


So, inside the game loop:

while (true)
{
    var oldState = State;
    State = HandleFrame(oldState);
    Render(State);
}


Besides handling and rendering the states inside the loop, it also checks whether the game is over or not depending on the GameState, and also calculates the time for the thread to sleep to maintain ~30FPS.

Other Classes:
GameObjects.cs → Parent Class for all Objects in the game: Hero, Bullet, Invader.
GameBoard.cs → Where all the UI drawing happens (render Hero, Invaders, Bullets, Score).
WelcomeScreen.cs → Game intro and dialogues with Captain + instructions.
ScoreManager.cs → Manage scores: add, search, delete, statistics, save/load file.
PlayerManager.cs → Manage players (Name + Score), show Top 10 players.

Features added in this version:
Main Menu with 4 options:
Play Game (with difficulty selection: Easy / Medium / Hard).
View Scoreboard (search, delete, statistics, sort).
View Top 10 players (Name + Score, sorted).
Exit.
Save & Load Scoreboard from scores.txt file.
Top 10 Ranking: sorted by highest score.
Scoreboard Menu:
Search score (linear search).
Delete score by index.
Statistics (games played, highest score, average).
Sort scores descending (Bubble Sort).

Future Works:
Implement ASCII animation when the bullet hits the invader.
Add background music and bullet firing sounds.
Allow enemies to fire bullets (currently only the player can).
Introduce different fighter jets with unique features.
Make game objects bigger and let them take more grid space.