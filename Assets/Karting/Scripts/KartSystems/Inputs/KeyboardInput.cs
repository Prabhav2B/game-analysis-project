using UnityEngine;

namespace KartGame.KartSystems {

    public class KeyboardInput : BaseInput
    {
        public string Horizontal = "Horizontal";
        public string Vertical = "Vertical";
        private float yaxis;
        public bool controller;
        public override Vector2 GenerateInput() 
        {
            if(controller)
            {
                if(Input.GetKey(KeyCode.Joystick1Button5))
                {
                    yaxis = 1;
                }
                else
                {
                    yaxis = 0;
                }
                return new Vector2
                {
                    x = Input.GetAxis(Horizontal),
                    y = yaxis
                };
            }

            return new Vector2
            {
                x = Input.GetAxis(Horizontal),
                y = Input.GetAxis(Vertical)
            };
        }
    }
}
