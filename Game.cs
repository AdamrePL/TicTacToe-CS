namespace TTT
{
    class Game
    {
        /**
         * TODO:
         * Make documentation [0]
         * Add comments as a documentation inside code [0.0.1]
         * Sort out methods [0.0.1]
         * ! Add input handlers? I doubt I need it. I guess maybe a feedback [0.1.0]
         * ! Add tests for everything; ex. execution time for each method [0.1.0]
         * ! Check if anything can be optimized; reduce memory and increase speed [0.0.1]
         * ! Improve game execution script [0.1.0]
         * 
         * & After 1.0.0 release:
         * Add games history [0.1.0]
         */
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
            _game_states.Add(3, "Draw!");
        }

        private void mark_field(Index field, bool turn)
        {
            this._board[field] = turn;
        }

        public Game make_move(byte field)
        {
            if (field != 0) { field--; }
            if (this.is_legal(field))
            {
                this._turn = !this._turn;
                this.mark_field(field, this._turn);
            }
            return this;
        }

        private bool is_legal(byte field)
        {
            return this._board[field] == null;
        }

        private byte check_game_state()
        {
            if (!this._board.Contains(null)) return 3;
            foreach (Index[] condition in _win_conds)
            {
                if (condition.Count(index => _board[index] == false) == 3) return 2;
                if (condition.Count(index => _board[index] == true) == 3) return 1;
            }
            return 0;
        }

        public string get_verbalized_game_state()
        {
            return this._game_states[check_game_state()];
        }

        public bool is_finished()
        {
            return this.check_game_state() > 0;
        }

        public void display_board(List<bool?>? provided = null)
        {
            var board = provided != null ? provided : this._board;
            List<char> normalized_board = new();
            foreach (bool? field in board)
            {
                normalized_board.Add(field == null ? ' ' : (bool)field ? this.player_1 : this.player_2);
            }
            Console.WriteLine('-' + new string('-', (int)(board.Count / 1.5 * 2)));
            for (int i = 0; i < board.Count; i += 3)
            {
                string str = "";

                for (int j = 0; j < 3; j++)
                {
                    str += this._tips ? normalized_board[i + j] == ' ' ? (i + j + 1).ToString() : board[i + j] : board[i + j];
                    str += j < 2 ? " | " : "";
                }

                Console.WriteLine("| " + str + " |");
                Console.WriteLine('-' + new string('-', (int)(board.Count / 1.5 * 2)));
            }
        }
    }
    class Program
    {
        public static bool tips;
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Play with numbered fields? (y/n)");
                string? tips_prompt = Console.ReadLine();
                if (tips_prompt.Contains("exit")) break;
                switch (tips_prompt)
                {
                    case "y":
                        tips = true;
                        break;
                    case "n":
                        tips = false;
                        break;
                }
                Game game = new(tips);
                Console.Clear();
                game.display_board();

                while (!game.is_finished())
                {
                    string? prompt = Console.ReadLine();
                    if (prompt.Contains("cancel")) break;
                    if (prompt.Length == 1 && char.IsDigit(prompt[0]))
                    {
                        Console.Clear();
                        game.make_move(byte.Parse(prompt)).display_board();
                    }
                }
                if (game.is_finished()) Console.WriteLine(game.get_verbalized_game_state());
            }
        }
    }
}
