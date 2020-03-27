using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game2.Screens
{
    class Screen1 : Screen
    {
        AnimatedEntity entity;
        TextEntity txtEntity;
        
        //Construtor da Screen1
        public Screen1(ScreenManager manager, string name) : base(manager: manager, name, true)
        {

        }

        //Método para carregamento da tela.
        public override void Load()
        {
            //Criação de uma entidade dentro da dela.
            AnimatedEntity shadow = AnimatedEntity.CreateRectangle(Game, nameof(shadow), new Point(100, 100), new Color(50, 50, 50, 50), this);
            //Alinha a entidade ao centro da tela.
            shadow.Transform.SetViewPosition(AlignType.Center);

            //Criação de uma entidade nomeada fora da tela.
            entity = AnimatedEntity.CreateRectangle(Game, nameof(entity), new Point(100, 100), Color.DarkBlue, this);

            //OBS.: A entidade poderia ser iniciada como:
            //entity = AnimatedEntity.CreateRectangle(Game, nameof(entity), new Point(100, 100), Color.DarkBlue);
            //Removendo o "this" no final que referencia esta tela.
            //Deve-se então, no final do método Load() adicionar manualmente a entidade com o método base.Add(entity).

            entity.Origin = new Vector2(entity.Transform.Width / 2, entity.Transform.Height /2);
            entity.Transform.SetViewPosition(AlignType.Center);    
            
            //Atualiza a entidade utilizando o evento OnUpdate
            entity.OnUpdate += Entity_OnUpdate;            

            //Entidade para exibição de um texto.
            txtEntity = new TextEntity(this, nameof(txtEntity));
            txtEntity.SetFont("default");
            txtEntity.Text.Append("Teste de movimentação.");
            txtEntity.Text.Append("\n");
            txtEntity.Text.Append("Pressione as setas do teclado para movimentação e A e S para rotação.");
            txtEntity.Text.Append("\n");
            txtEntity.Text.Append("Pressione Space para mudar de tela.");

            base.Load();
        }

        private void Entity_OnUpdate(Entity2D source, GameTime gameTime)
        {
            //Código para movimento e rotação da entidade.

            var input = Manager.Input;

            if(input.Keyboard.IsDown(Keys.Right))
            {
                source.Transform.Move(new Vector2(3, 0));
            }
            if (input.Keyboard.IsDown(Keys.Left))
            {
                source.Transform.Move(new Vector2(-3, 0));
            }
            if (input.Keyboard.IsDown(Keys.Up))
            {
                source.Transform.Move(new Vector2(0, -3));
            }
            if (input.Keyboard.IsDown(Keys.Down))
            {
                source.Transform.Move(new Vector2(0, 3));
            }
            if (input.Keyboard.IsDown(Keys.A))
            {
                //A rotação tem que ser em radianos.
                source.Transform.Rotation += MathHelper.ToRadians(2);
            }
            if (input.Keyboard.IsDown(Keys.S))
            {
                //A rotação tem que ser em radianos.
                source.Transform.Rotation -= MathHelper.ToRadians(2);
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

            entity.Transform.SetViewPosition(AlignType.Center);
            entity.Transform.Rotation = 0;

            base.Reset();
        }
    }
}