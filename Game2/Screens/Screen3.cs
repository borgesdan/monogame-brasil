using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game2.Screens
{
    class Screen3 : Screen
    {
        AnimatedEntity entity;
        TextEntity txtEntity;

        public Screen3(ScreenManager manager, string name) : base(manager: manager, name, true)
        {

        }

        //Método para carregamento da tela.
        public override void Load()
        {
            AnimatedEntity shadow = AnimatedEntity.CreateRectangle(Game, nameof(shadow), new Point(100, 100), new Color(50, 50, 50, 50), this);
            shadow.Transform.SetViewPosition(AlignType.Center);

            //Criação de uma entidade nomeada fora da tela.
            entity = AnimatedEntity.CreateRectangle(Game, nameof(entity), new Point(100, 100), Color.DarkBlue, this);
            entity.Origin = new Vector2(entity.Transform.Width / 2, entity.Transform.Height / 2);
            entity.Transform.SetViewPosition(AlignType.Center);
            //Atualiza a entidade utilizando o evento OnUpdate
            entity.OnUpdate += Entity_OnUpdate;

            //Criamos um componente para verificar se a entidade saiu dos limites da tela.
            OutOfBoundsComponent outOfBoundsComponent = new OutOfBoundsComponent(this.MainViewport.Bounds);
            outOfBoundsComponent.OnOutOfBounds += (Entity2D source, GameTime gameTime, Vector2 result) =>
            {
                //Movemos então a entidade na direção contrária.
                entity.Transform.Move(result);

                //E invertemos sua velocidade.
                if (result.X != 0)
                    entity.Transform.InvertVelocityX();
                if (result.Y != 0)
                    entity.Transform.InvertVelocityY();
            };

            entity.Components.Add(outOfBoundsComponent);
            entity.Transform.SetVelocity(3, 3);

            //Entidade para exibição de um texto.
            txtEntity = new TextEntity(this, nameof(txtEntity));
            txtEntity.SetFont("default");
            txtEntity.Text.Append("Teste de saida de tela");
            txtEntity.Text.Append("\n");
            txtEntity.Text.Append("Pressione as setas do teclado para movimentacao e A e S para rotacao.");
            txtEntity.Text.Append("\n");
            txtEntity.Text.Append("Pressione Space para mudar de tela.");

            base.Load();
        }

        private void Entity_OnUpdate(Entity2D source, GameTime gameTime)
        {
            //Código para movimento e rotação da entidade.

            var input = Manager.Input;
            
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
            entity.Transform.InvertVelocity();

            base.Reset();
        }
    }
}
