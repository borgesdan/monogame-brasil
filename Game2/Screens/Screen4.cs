using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game2.Screens
{
    class Screen4 : Screen
    {
        AnimatedEntity entity;
        TextEntity txtEntity;

        public Screen4(ScreenManager manager, string name) : base(manager: manager, name, true)
        {

        }

        public override void Load()
        {
            entity = new AnimatedEntity(this, "mario");

            Animation mario_idle = new Animation(Game, 0, nameof(mario_idle));
            mario_idle.AddSprite("mario");

            entity.AddAnimation(mario_idle);            
            entity.Origin = new Vector2(entity.Transform.Width / 2, entity.Transform.Height / 2);            
            entity.Transform.SetViewPosition(AlignType.Center);
            entity.Transform.Scale = new Vector2(0.5F, 0.5F);
            entity.OnUpdate += Entity_OnUpdate;
            
            txtEntity = new TextEntity(this, nameof(txtEntity));
            txtEntity.SetFont("default");
            txtEntity.Text.Append("Teste de animação estática.");
            txtEntity.Text.Append("\n");
            txtEntity.Text.Append("Pressione as teclas Q e W para escala.");
            txtEntity.Text.Append("\n");
            txtEntity.Text.Append("Pressione Space para mudar de tela.");

            base.Load();
        }

        private void Entity_OnUpdate(Entity2D source, GameTime gameTime)
        {
            //Código para escala da entidade.

            var input = Manager.Input;

            if (input.Keyboard.IsDown(Keys.Q))
            {
                source.Transform.Scale += new Vector2(0.2f, 0.2F);

                if (source.Transform.Scale.X > 2F)
                {
                    source.Transform.Scale = new Vector2(2, 2);                    
                }
            }
            if (input.Keyboard.IsDown(Keys.W))
            {
                source.Transform.Scale += new Vector2(-0.2f, -0.2F);
                
                if (source.Transform.Scale.X < 0)
                {
                    source.Transform.Scale = new Vector2(0.1F, 0.1F);
                }                
            }
        }

        //Sobrecarga do método Update da tela.
        public override void Update(GameTime gameTime)
        {
            var input = Manager.Input;

            //Mudar para próxima tela da lista
            if (input.Keyboard.IsPress(Keys.Space))
            {
                Manager.Next(true);

                //Poderia usar também o método Change() informando o nome da tela.
            }

            base.Update(gameTime);
        }

        //Sobrecarga do método Reset
        public override void Reset()
        {
            //Informos aqui o que acontece quando a tela for resetada.            
            entity.Transform.Scale = new Vector2(0.5F, 0.5F);

            base.Reset();
        }
    }
}