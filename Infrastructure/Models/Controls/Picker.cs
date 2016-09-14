using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Models.Screens;

namespace Infrastructure.Models.Controls
{
    public class Picker<T> : Control
    {
        private readonly List<KeyValuePair<string, T>> r_Options = new List<KeyValuePair<string, T>>();

        public List<KeyValuePair<string, T>> Options
        {
            get { return r_Options; }
        }

        private KeyValuePair<string, T> m_SelectedOption;

        public KeyValuePair<string, T> SelectedOption
        {
            get
            {
                if (m_SelectedIndex >= 0 && m_SelectedIndex < r_Options.Count)
                {
                    return m_SelectedOption;
                }

                throw new Exception("There is no selected option");
            }

            private set
            {
                if(!SelectedOption.Equals(value))
                {
                    m_SelectedOption = value;
                    OnSelectedOptionChanged();
                }
            }
        }

        private int m_SelectedIndex = 0;
        private string m_Title;

        public event EventHandler<PickerEventArgs<T>> SelectedOptionChanged;

        public Picker(GameScreen i_GameScreen, string i_Name, string i_Title, Text i_Text)
            : base(i_GameScreen, i_Name, i_Text)
        {
            m_Title = i_Title;
        }

        public Picker(GameScreen i_GameScreen, string i_Name, string i_Title, Text i_Text, Sprite i_Texture)
            : base(i_GameScreen, i_Name, i_Text, i_Texture)
        {
            m_Title = i_Title;
        }

        public override void Initialize()
        {
            base.Initialize();

            m_SelectedOption = r_Options[m_SelectedIndex];
            setTextContent();
        }

        public override void Update(GameTime i_GameTime)
        {
            if (KeyboardManager.IsKeyPressed(Keys.PageDown) || MouseManager.MouseWheelDelta < 0 || MouseManager.IsKeyPressed(eMouseButton.RightButton))
            {
                m_SelectedIndex++;
                if (m_SelectedIndex >= r_Options.Count)
                {
                    m_SelectedIndex = 0;
                }

                SelectedOption = r_Options[m_SelectedIndex];
            }
            else if (KeyboardManager.IsKeyPressed(Keys.PageUp) || MouseManager.MouseWheelDelta > 0)
            {
                m_SelectedIndex--;

                if (m_SelectedIndex < 0)
                {
                    m_SelectedIndex = r_Options.Count - 1;
                }

                SelectedOption = r_Options[m_SelectedIndex];
            }

            base.Update(i_GameTime);
        }

        public void SetSelectionOption(T i_Value)
        {
            int index = r_Options.FindIndex((option) => i_Value.Equals(option.Value));

            if(index >= 0)
            {
                m_SelectedIndex = index;
                SelectedOption = r_Options[m_SelectedIndex];
            }
        }

        protected virtual void OnSelectedOptionChanged()
        {
            if(SelectedOptionChanged != null)
            {
                SelectedOptionChanged.Invoke(this, new PickerEventArgs<T>(SelectedOption));
            }

            setTextContent();
        }

        private void setTextContent()
        {
            Text.Content = string.Format("{0}: {1}", m_Title, SelectedOption.Key);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                SelectedOptionChanged = null;
            }

            base.Dispose(disposing);
        }
    }

    public class PickerEventArgs<T> : EventArgs
    {
        private KeyValuePair<string, T> r_SelectedOption;

        public PickerEventArgs(KeyValuePair<string, T> i_SelectedOption)
        {
            r_SelectedOption = i_SelectedOption;
        }

        public T SelectedValue
        {
            get { return r_SelectedOption.Value; }
        }

        public string SelectedOption
        {
            get { return r_SelectedOption.Key; }
        }
    }
}