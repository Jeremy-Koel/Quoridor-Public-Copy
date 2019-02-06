using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{


    /// <summary>
    /// Class Name: MonteCarloNode
    /// Description: MonteCarloNode is a node to be used in the building of a Monte Carlo Search Tree. The constructor for a Monte Carlo node
    /// accepts a Gameboard.
    /// </summary>
    class MonteCarloNode
    {
        private List<MonteCarloNode> children;
        private MonteCarloNode parent;
        private double winRate;
        private int timesVisited;
        public GameBoard boardState;
        private string thisMove;

        public MonteCarloNode(GameBoard newState)
        {
            boardState = new GameBoard(newState);
            children = new List<MonteCarloNode>();
            parent = null;
        }

        public MonteCarloNode(MonteCarloNode childParent, GameBoard newState, string move)
        {
            parent = childParent;
            boardState = newState;
            thisMove = move;
            if (!move.Contains("v") && !move.Contains("h"))
            {
                boardState.MovePiece(boardState.GetWhoseTurn() == 1 ? GameBoard.PlayerEnum.ONE : GameBoard.PlayerEnum.TWO, new PlayerCoordinate(move));
            }
            boardState.PrintBoard();
        }

        public List<MonteCarloNode> GetChildrenNodes()
        {
            return children;
        }

        public MonteCarloNode GetParentNode()
        {
            return parent;
        }

        /// <summary>
        /// <c>InsertChild</c> method adds a new MonteCarloNode to the MonteCarlo Tree by taking in a move determined as a string
        /// and converting it into its proper GameBoard state.If the move provided places a wall, a new instance of GameBoard is
        /// created. Otherwise a normal pawn move will result in the current node's GameBoard being used.
        /// </summary>
        /// <param name="move">specified move - either place a wall or move a pawn</param>
        public void InsertChild(string move)
        {
            if (move.Contains("v") || move.Contains("h"))
            {
                GameBoard newState = new GameBoard(boardState);
                newState.PlaceWall(newState.GetWhoseTurn() == 1 ? GameBoard.PlayerEnum.ONE : GameBoard.PlayerEnum.TWO, new WallCoordinate(move));
                children.Add(new MonteCarloNode(this, newState, move));
            }
            else
            {
                 children.Add(new MonteCarloNode(this, boardState, move));
            }
        }

    }
    class MonteCarlo
    {
        static MonteCarlo()
        {
            MonteCarloNode insertTest = new MonteCarloNode(new GameBoard(GameBoard.PlayerEnum.ONE, "e1", "e9"));

            insertTest.InsertChild("e2");
            insertTest.InsertChild("e4h");
            insertTest.InsertChild("e6v");
        }

    }
}
