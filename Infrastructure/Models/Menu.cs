using System;
using System.Collections.Generic;
using Infrastructure.ManagersInterfaces;
using Infrastructure.Models.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.Models
{
    public class Menu : CompositeDrawableComponent<Control>
    {
        public event EventHandler<EventArgs> ActiveControlChanged;

        private IKeyboardManager m_KeyboardManager;
        private IMouseManager m_MouseManager;
        private int m_CurrentControlIdx = -1;

        private bool m_MouseMoved = false;

        public Control CurrentControl
        {
            get
            {
                return r_ControlsList[m_CurrentControlIdx];
            }

            set
            {
                if (m_CurrentControlIdx > -1 && r_ControlsList[m_CurrentControlIdx] != value)
                {
                    r_ControlsList[m_CurrentControlIdx].Enabled = false;
                    m_CurrentControlIdx = r_ControlsList.IndexOf(value);

                    OnActiveControlChanged();

                    if (value != null)
                    {
                        value.Enabled = true;
                    }
                }
            }
        }

        private Vector2 m_Position;

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        private int m_Margin = 50;

        public int Margin
        {
            get { return m_Margin; }
            set { m_Margin = value; }
        }

        private int m_Height = 0;
        
        public int Height
        {
            get { return m_Height; }
        }

        private int m_Width = 0;

        public int Width
        {
            get { return m_Width; }
            set
            {
                if (m_Width != value)
                {
                    m_Width = value;

                    foreach(Control control in r_ControlsList)
                    {
                        control.Width = Width;
                    }
                }
            }
        }

        protected readonly Dictionary<string, Control> r_ControlsDictionary;
        protected readonly List<Control> r_ControlsList;

        public Menu(Game i_Game) : base(i_Game)
        {
            r_ControlsDictionary = new Dictionary<string, Control>();
            r_ControlsList = new List<Control>();
        }

        public Control this[string i_Name]
        {
            get
            {
                Control retVal = null;
                r_ControlsDictionary.TryGetValue(i_Name, out retVal);
                return retVal;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            m_KeyboardManager = (IKeyboardManager)Game.Services.GetService(typeof(IKeyboardManager));
            m_MouseManager = (IMouseManager)Game.Services.GetService(typeof(IMouseManager));

            bool isFirst = true;

            foreach(Control control in r_ControlsList)
            {
                control.Width = Width;
                control.Position += Position + new Vector2(0, m_Height + (isFirst ? 0 : Margin));

                m_Height = (int)(control.Position.Y - Position.Y + control.Height);

                isFirst = false;
            }
        }

        protected override void OnComponentAdded(GameComponentEventArgs<Control> e)
        {
            r_ControlsDictionary.Add(e.GameComponent.Name, e.GameComponent);
            r_ControlsList.Add(e.GameComponent);

            e.GameComponent.Enabled = false;

            base.OnComponentAdded(e);
        }

        protected override void OnComponentRemoved(GameComponentEventArgs<Control> e)
        {
            r_ControlsDictionary.Remove(e.GameComponent.Name);
            r_ControlsList.Remove(e.GameComponent);
            base.OnComponentRemoved(e);
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if(m_MouseManager.DeltaX != 0 || m_MouseManager.DeltaY != 0)
            {
                m_MouseMoved = true;
            }

            bool isKeyDownPressed = m_KeyboardManager.IsKeyPressed(Keys.Down);
            bool isKeyUpPressed = m_KeyboardManager.IsKeyPressed(Keys.Up);

            if (isKeyDownPressed || isKeyUpPressed)
            {
                if (m_CurrentControlIdx >= 0 && m_CurrentControlIdx < r_ControlsList.Count)
                {
                    r_ControlsList[m_CurrentControlIdx].Enabled = false;
                }

                m_MouseMoved = false;

                if (isKeyDownPressed)
                {
                    m_CurrentControlIdx++;
                    if (m_CurrentControlIdx >= r_ControlsList.Count)
                    {
                        m_CurrentControlIdx = 0;
                    }

                    OnActiveControlChanged();
                }
                else if (isKeyUpPressed)
                {
                    m_CurrentControlIdx--;
                    if (m_CurrentControlIdx < 0)
                    {
                        m_CurrentControlIdx = r_ControlsList.Count - 1;
                    }

                    OnActiveControlChanged();
                }

                if (m_CurrentControlIdx >= 0 && m_CurrentControlIdx < r_ControlsList.Count)
                {
                    r_ControlsList[m_CurrentControlIdx].Enabled = true;
                }
            }
            else if (m_MouseMoved)
            {
                for (int i = 0; i < r_ControlsList.Count; i++)
                {
                    if (r_ControlsList[i].IsMouseOn(m_MouseManager.X, m_MouseManager.Y) && i != m_CurrentControlIdx)
                    {
                        if (m_CurrentControlIdx != -1)
                        {
                            r_ControlsList[m_CurrentControlIdx].Enabled = false;
                        }

                        m_CurrentControlIdx = i;
                        r_ControlsList[i].Enabled = true;

                        OnActiveControlChanged();
                    }
                    else if (r_ControlsList[i].IsMouseOn(m_MouseManager.PrevX, m_MouseManager.PrevY) && !r_ControlsList[i].IsMouseOn(m_MouseManager.PrevX, m_MouseManager.PrevY))
                    {
                        r_ControlsList[i].Enabled = false;
                    }
                }
            }
            else
            {
            }

            base.Update(gameTime);
        }

        private void OnActiveControlChanged()
        {
            if(ActiveControlChanged != null)
            {
                ActiveControlChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }
}