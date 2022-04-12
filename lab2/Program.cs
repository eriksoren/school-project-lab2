using System;
using System.Globalization; 
using System.Collections.Generic;

namespace lab2
{
    class Program
    {

        static void Main(string[] args)
        {

            //Test input: "SHAPE,X,Y, LENGTH , POINTS ; CIRCLE ,3 ,1 ,13 ,100; CIRCLE ,1 , -1 ,15 ,200; SQUARE, -1 ,0 ,20 ,300; SQUARE , -3 ,2 ,8 ,400; ", "(0,0)");

            game new_game = new game(args[0], args[1]);

            // Failed to handle input that start -1 
        }
    }


    // instance of the shapes in input scheme
    class Figure
    {
        public string shape { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double length { get; set; }
        public double points { get; set; }
    }

    class Square : Figure
    {
        public double get_area()
        {
            double get_sides = this.length / 4;
            return get_sides * get_sides;
        }

        public double get_square()
        {
            return (get_area() / 4) / 2;
        }
    }

    class Circle : Figure
    {
        double pi = Math.PI;

        public double get_diameter()
        {
           
            return this.length / pi; 
        }

        public double get_radius()
        {
            return get_diameter() / 2; 
        }

        public double get_area()
        {
            return (get_radius() * get_radius()) * pi; 
        }
    }

    // class that provide logic to the program 
    class game_logic
    {
        private string scheme;
        private string header_scheme; 
        private string pointer;
        private double pointer_x;
        private double pointer_y;
        private double hit_scorecard;
        private double miss_scorecard;
        private int final_score; 

        List<Figure> shape_list = new List<Figure>();

        /* Setters */

        public void set_scheme(string input)
        {
            this.scheme = input;
        }

        public void set_header_scheme(string input)
        {
            this.header_scheme = input; 
        }

        public void set_pointer(string input)
        {
            this.pointer = input;
        }

        public void set_pointer_x(double pointer_x)
        {
            this.pointer_x = pointer_x;
        }

        public void set_pointer_y(double pointer_y)
        {
            this.pointer_y = pointer_y;
        }

        public void set_final_score(double score)
        {
            this.final_score = Convert.ToInt16(score); 
        }

        /* Getters */

        public double get_hit_scorecard()
        {
            return hit_scorecard; 
        }
        public double get_miss_scoredcard()
        {
            return miss_scorecard; 
        }

        public string get_scheme()
        {
            return scheme;
        }

        public string get_header_scheme()
        {
            return header_scheme; 
        }

        public string get_pointer()
        {
            return pointer;
        }

        public double get_pointer_x()
        {
            return pointer_x;
        }

        public double get_pointer_y()
        {
            return pointer_y;
        }

        public int get_final_score()
        {
            return final_score; 
        }

        /* helper function */


        // handle input for the x and y coordinate and convert to double 
        public void handle_input()
        {
            char[] chars_to_trim = { '(', ' ', ')' };
            string format_coord = pointer.Trim(chars_to_trim);

            string[] split_coord = format_coord.Split(",");

            set_pointer_x(Convert.ToDouble(split_coord[0]));
            set_pointer_y(Convert.ToDouble(split_coord[1]));
        }

        // method to return index for the specific header column 
        public int handle_scheme_order(string to_find)
        {
            string[] header_values = get_header_scheme().Split(",");

            for (int i = 0; i < (header_values.Length); i++)
            {
                if (string.Equals(header_values[i].Trim(), to_find))
                { 
                    return i;
                }
            }
     
            return 0;
        }

        // validate and create a new object figure from the input scheme
        public void handle_scheme()
        {
            string[] new_figures = scheme.Split(";");

            set_header_scheme(new_figures[0]); 

            for (int i = 0; i < (new_figures.Length - 1); i++)
            {
                if(i == 0)
                {
                   // Skip header row of the input scheme  
                    continue; 
                }

                string[] values = new_figures[i].Split(",");

                var format = new NumberFormatInfo();
                format.NegativeSign = "-";

                // Instance of obj

                string shapeString = values[handle_scheme_order("SHAPE")].Trim(); 

                if (shapeString.Equals("SQUARE", StringComparison.OrdinalIgnoreCase))
                {

                    Square shape = new Square(){};

                    shape.shape = values[handle_scheme_order("SHAPE")].Trim();
                    shape.x = double.Parse(values[handle_scheme_order("X")], format);
                    shape.y = double.Parse(values[handle_scheme_order("Y")], format);
                    shape.length = Convert.ToDouble(values[handle_scheme_order("LENGTH")]);
                    shape.points = Convert.ToDouble(values[handle_scheme_order("POINTS")]);
                    shape_list.Add(shape);


                } else
                {
                    Circle shape = new Circle(){};

                    shape.shape = values[handle_scheme_order("SHAPE")].Trim();
                    shape.x = double.Parse(values[handle_scheme_order("X")], format);
                    shape.y = double.Parse(values[handle_scheme_order("Y")], format);
                    shape.length = Convert.ToDouble(values[handle_scheme_order("LENGTH")]);
                    shape.points = Convert.ToDouble(values[handle_scheme_order("POINTS")]);
                    shape_list.Add(shape);

                }
        
            }

        }

        // Update the scorecard depended on what kind of shape and if it's a hit or miss 
        public void update_scorecard(string shape, double score, double area, bool hit)
        {
            if (hit)
            {
                if (shape.Equals("CIRCLE"))
                {
                    hit_scorecard = hit_scorecard + (2 * score) / area;
                }
                else
                {
                    hit_scorecard = hit_scorecard + (1 * score) / area;
                }
            }
            else
            {
                if (shape.Equals("CIRCLE"))
                {
                    miss_scorecard = miss_scorecard + (2 * score) / area;
                }
                else
                {
                    miss_scorecard = miss_scorecard + (1 * score) / area;
                }
            }
        }

        // Math method to multiply the number with itself
        public double calculate_elevate_number(double number)
        {
            return number * number;
        }

        // method to calculate if it was an hit or miss on the specific circle 
        public void calculate_circle(Circle shape)
        {

            //get diameter 

            double circle_diameter = shape.get_diameter();

            //get radius 

            double radius = shape.get_radius();

            //area 

            double area = shape.get_area();

            // Calculate circle hit

            if (calculate_elevate_number(shape.x) - calculate_elevate_number(get_pointer_x()) + calculate_elevate_number(shape.y) - calculate_elevate_number(get_pointer_y()) < calculate_elevate_number(radius))
            {
                update_scorecard(shape.shape, shape.points, area, true);
            }
            else
            {
                update_scorecard(shape.shape, shape.points, area, false);
            }
        }

        // method to calculate if it was a hit or miss on the square

       public void calculate_square(Square shape)
        {

            double square_area = shape.get_area();

            double square_x = shape.get_square();

            if (Math.Abs(shape.x - get_pointer_x()) < square_x & Math.Abs(shape.y - get_pointer_y()) < square_x)
            {
                update_scorecard(shape.shape, shape.points, square_area, true);
            }
            else
            {
                update_scorecard(shape.shape, shape.points, square_area, false);
            }


        }

        // calculate each figure and when all shapes are done do final_score
        public void calculate_score()
        {

            foreach (var obj in shape_list)
            {

                if (obj.shape.Equals("CIRCLE"))
                {
                    calculate_circle((Circle)obj); 

                } else if(obj.shape.Equals("SQUARE"))
                {
                    calculate_square((Square)obj); 
                }
            }

            calculate_final_score(); 
        }

        // set the final score for the instance of the class game
        public void calculate_final_score()
        {
            double sum = hit_scorecard - (miss_scorecard / 4);
            set_final_score(Math.Round(sum)); 
        }
    }
        class game
        {
            game_logic game_logic = new game_logic();

            public game(string pointer, string scheme)
            {
                game_validation validation = new game_validation(pointer, scheme);

                if(validation.get_validation())
                {

                    setup_game(pointer, scheme);
                    final_score();

                }

            }

            public void setup_game(string pointer, string scheme)
            {
                game_logic.set_pointer(pointer);
                game_logic.set_scheme(scheme);

                game_logic.handle_input();
                game_logic.handle_scheme();
            }

            public void final_score()
            {
                game_logic.calculate_score();
                Console.WriteLine("Final score: " + game_logic.get_final_score());
            }
        }

        class game_validation
    {
        private bool validation = false; 
        // constructor for game_validation

        public game_validation(string pointer, string scheme)
        {
            try
            {
   
                if(validate_pointer(pointer) && validate_scheme(scheme))
                {
                    set_validation(); 
                }

            } catch(ArgumentNullException e)
            {
                Console.WriteLine("An error occur: {0} Please try again!", e.Message);
                Environment.Exit(0); 
            } catch(InvalidOperationException e)
            {
                Console.WriteLine("An error occur: {0} Please try again!", e.Message);
                Environment.Exit(0); 
            } catch(ArgumentException e)
            {
                Console.WriteLine("An error occur: {0} Please try again!", e.Message);
                Environment.Exit(0); 
            }
        }

        // setter for validation

        public void set_validation()
        {
            this.validation = true; 
        }

        // getter for validation
        public bool get_validation()
        {
            return validation; 
        }

        // validation for pointer
        public bool validate_pointer(string pointer)
        {
            if (String.IsNullOrEmpty(pointer))
            {
                throw (new ArgumentNullException());
            }
            else if (pointer.Contains(",") == false)
            {
                throw (new InvalidOperationException("Coordinates must contain a , separator. Valid input example: (0,1)."));
            }

            return true; 
        }
        // validation for scheme
        public bool validate_scheme(string scheme)
        {
            string[] figure_list = scheme.Split(";");

            for (int i = 0; i < (figure_list.Length - 1); i++)
            {
                string[] figure_list_row = figure_list[i].Split(",");

                if(figure_list_row.Length != 5)
                {
                    throw (new ArgumentException("Missing value in scheme")); 
                }
            }


                if (String.IsNullOrEmpty(scheme))
            {
                throw (new ArgumentNullException());

            } else if (scheme.Contains(",") == false)
            {
                throw (new InvalidOperationException("Scheme must contain a, separator.")); 
            }
            return true; 
        }
    }
}
