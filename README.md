U-RSP (Ultimate Rock Scissor Paper) is an extended version of the classic Rock-Paper-Scissors game. It allows players to define an arbitrary odd number of non-repeating moves, creating a unique set of rules for each game. The game uses HMAC to ensure fair play by providing a hash of the computer's move before the player makes their move.

Features:
#Supports any odd number of moves (≥ 3).
#Customizable move names.
#Displays error messages for invalid input.
#Determines winners based on the circular order of moves.
#Ensures fairness using HMAC for move validation.
#The game will display the available moves and prompt the player to choose a move.
#The player can enter the number corresponding to their move, 0 to exit, or ? to display the help table.

Requirements:
#BouncyCastle.Crypto package
#ConsoleTables package

command:
dotnet run -- Rock Paper Scissors Lizard Spock
