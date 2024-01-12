using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace NPCAttacker.UI
{
    public class UIClickableButton : UIElement
    {
        private string _text;
        private MouseEvent _clickAction;
        private UIPanel _uiPanel;
        private UIText _uiText;

        public string Text
        {
            get => _uiText.Text;
            set => _text = value;
        }

        public UIClickableButton(string text, MouseEvent clickAction) : base()
        {
            _text = text;
            _clickAction = clickAction;
        }

        public override void OnInitialize()
        {
            _uiPanel = new UIPanel
            {
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill
            };
            Append(_uiPanel);

            _uiText = new UIText("");
            _uiText.VAlign = _uiText.HAlign = 0.5f;
            _uiPanel.Append(_uiText);
            _uiPanel.OnLeftClick += _clickAction;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _uiText.SetText(_text);
            Recalculate();
            MinWidth = _uiText.MinWidth;
            MinHeight = _uiText.MinHeight;
        }
    }
}
