using System.Collections.Immutable;

namespace TTT
{
    class Game
    {
        private List<bool?> _board = [];
        private bool _tips;
        private bool _turn = false;
        private readonly List<Index[]> _win_conds = [
            [0, 1, 2], [3, 4, 5], [6, 7, 8],
            [0, 3, 6], [1, 4, 7], [2, 5, 8],
            [0, 4, 8], [2, 4, 6]
        ];
        private readonly Dictionary<byte, string> _game_states = [];

        protected char player_1 = 'X';
        protected char player_2 = 'O';

        public Game(bool tips = false)
        {
            this._tips = tips;
            for (byte i = 0; i < 9; i++)
            {
                this._board.Add(null);
            }

            _game_states.Add(0, "ongoing");
            _game_states.Add(1, $"{player_1} Won!");
            _game_states.Add(2, $"{player_2} Won!");
            _game_states.Add(3, "draw!");
        }

        private void mark_field(Index field, bool turn)
        {
            this._board[field] = turn;
        }

        public Game make_move(byte field)
        {
            field--;
            if (this.is_legal(field))
            {
                this._turn = !this._turn;
                this.mark_field(field, this._turn);
            }
            return this;
        }

        public bool is_legal(byte field)
        {
            return this._board[field] == null ? true : false;
        }

        private byte check_game_state()
        {
            return 2;
        }

        public string send_game_state_info(byte game_state)
        {
            return _game_states[check_game_state()];
        }

        private List<char> getReadable_board()
        {
            List<char> result = new();
            foreach (bool? field in this._board)
            {
                result.Add(field == null ? ' ' : (bool)field ? this.player_1 : this.player_2);
            }
            return result;
        }

        public void display_board(List<char>? provided = null)
        {
            var board = provided != null ? provided : this.getReadable_board();
            Console.WriteLine('-' + new string('-', (int)(board.Count / 1.5 * 2)));
            for (int i = 0; i < board.Count; i += 3)
            {
                string str = "";

                for (int j = 0; j < 3; j++)
                {
                    str += this._tips ? board[i + j] == ' ' ? (i + j + 1).ToString() : board[i + j] : board[i + j];
                    str += j < 2 ? " | " : "";
                }

                Console.WriteLine("| " + str + " |");
                Console.WriteLine('-' + new string('-', (int)(board.Count / 1.5 * 2)));
            }
            Console.WriteLine();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new();
            game.display_board();
            game.make_move(5);
            game.display_board();
            Console.WriteLine(game.send_game_state_info(2));
        }
    }
}

/*
 * game.display_board(new() 
            {
                'O', 'X', 'E',         
                'O', 'O', 'X',
                'X', 'X', 'O',
            });
*/
