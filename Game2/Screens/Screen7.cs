using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2.Screens
{
    class Screen7 : Screen
    {
        TextEntity txtEntity;

        public Screen7(ScreenManager manager, string name) : base(manager: manager, name, true)
        {
            BackgroundColor = Color.DarkBlue;
        }

        public override void Load()
        {
            Animation floor_animation = new Animation(Game, 0, "floor");
            Sprite floor_sprite = new Sprite(Game, "80800", false);
            floor_sprite.AddFrame(490, 971, 1342, 90);
            floor_animation.AddSprite(floor_sprite);

            ScreenLayer floor_layer = new ScreenLayer(this);
            floor_layer.AddAnimation(floor_animation);
            floor_layer.SetPosition(new Point(0, 390));

            Animation floor_animation2 = new Animation(Game, 0, "floor");
            Sprite floor_sprite2 = new Sprite(Game, "80800", false);
            floor_sprite2.AddFrame(490, 971, 1342, 90);
            floor_animation2.AddSprite(floor_sprite);

            ScreenLayer floor_layer2 = new ScreenLayer(this);
            floor_layer2.AddAnimation(floor_animation2);
            floor_layer2.SetPosition(new Point(0, 190));
            floor_layer2.Parallax = 0.5f;
            

            base.BackLayers.Add(floor_layer2);
            base.BackLayers.Add(floor_layer);

            //Entidade para exibição de um texto.
            //Inicializamos a entidade sem passar a tela no construtor.
            txtEntity = new TextEntity(Game, nameof(txtEntity));
            txtEntity.SetFont("default");
            txtEntity.Text.Append("7: Teste de movimentação da câmera e efeito parallax.");
            txtEntity.Text.Append("\n");
            txtEntity.Text.Append("Pressione as setas do teclado para movimentação");
            txtEntity.Text.Append("\n");
            txtEntity.Text.Append("Pressione Space ou Backspace para mudar de tela.");

            TextEntity txtParallax = new TextEntity(Game, nameof(txtEntity));
            txtParallax.SetFont("default");
            txtParallax.Text.Append("Com parallax:");
            txtParallax.Transform.Y = 170;
            txtParallax.Transform.Color = Color.Yellow;

            TextEntity txtNormal = new TextEntity(Game, nameof(txtEntity));
            txtNormal.SetFont("default");
            txtNormal.Text.Append("Sem parallax:");
            txtNormal.Transform.Y = 370;
            txtNormal.Transform.Color = Color.Yellow;

            //Adicionamos a entidade no grupo de entidades estáticas frontais.
            //Esse grupo não é afetado pela câmera.
            base.AddFrontStaticEntity(txtEntity, txtParallax, txtNormal);

            base.Load();
        }

        //Sobrecarga do método Update da tela.
        public override void Update(GameTime gameTime)
        {
            var input = Manager.Input;

            if (input.Keyboard.IsDown(Keys.Right))
            {
                camera.Move(5, 0);
            }
            if (input.Keyboard.IsDown(Keys.Left))
            {
                camera.Move(-5, 0);
            }
            if (input.Keyboard.IsDown(Keys.Up))
            {
                camera.Move(0, -5);
            }
            if (input.Keyboard.IsDown(Keys.Down))
            {
                camera.Move(0, 5);
            }

            //Mudar para próxima tela da lista
            if (input.Keyboard.IsPress(Keys.Space))
            {
                Manager.Next(true);

                //Poderia usar também o método Change() informando o nome da tela.
            }
            if (input.Keyboard.IsPress(Keys.Back))
            {
                Manager.Back(true);

                //Poderia usar também o método Change() informando o nome da tela.
            }


            base.Update(gameTime);
        }

        //Sobrecarga do método Reset
        public override void Reset()
        {
            base.Reset();
        }
    }
}
