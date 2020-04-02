using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game2.Screens
{
    class Screen2 : Screen
    {
        AnimatedEntity entity;
        TextEntity txtEntity;

        public Screen2(ScreenManager manager, string name) : base(manager: manager, name, true)
        {

        }

        public override void Load()
        {   
            entity = AnimatedEntity.CreateRectangle(Game, nameof(entity), new Point(100, 100), Color.DarkBlue);
            entity.Origin = new Vector2(entity.Transform.Width / 2, entity.Transform.Height / 2);
            entity.Transform.SetViewPosition(AlignType.Center);            
            entity.OnUpdate += Entity_OnUpdate;

            //Adicionamos um componente para verificar a colisão com as entidades da tela.
            BasicCollisionComponent basicCollisionComponent = new BasicCollisionComponent();
            //Utilizamos um método lambda.
            basicCollisionComponent.OnCollision += (Entity2D source, GameTime gameTime, CollisionResult result, Entity2D collidedEntity) =>
            {
                //Não calcular se a entidade colidida for a 'shadow' e a 'txtEntity'
                if (collidedEntity.Name == "shadow" || collidedEntity.Name == "txtEntity")
                    return;

                //Colisão de retângulos.
                if(result.Type == CollisionType.Rectangle)
                {
                    source.Transform.Move(result.RectangleResult.Subtract);
                }
                //Colisão de retângulos rotacionados.
                else if (result.Type == CollisionType.Polygon)
                {
                    source.Transform.Move(result.PolygonResult.Subtract);
                }
            };

            //Adicionamos o componente
            entity.Components.Add(basicCollisionComponent);

            txtEntity = new TextEntity(Game, nameof(txtEntity));
            txtEntity.SetFont("default");
            txtEntity.Text.Append("2: Teste de colisão.");
            txtEntity.Text.Append("\n");
            txtEntity.Text.Append("Pressione as setas do teclado para movimentação e A e S para rotação.");
            txtEntity.Text.Append("\n");
            txtEntity.Text.Append("Q e W para escala.");

            AnimatedEntity shadow = AnimatedEntity.CreateRectangle(Game, nameof(shadow), new Point(100, 100), new Color(50, 50, 50, 50));
            shadow.Transform.SetViewPosition(AlignType.Center);

            AnimatedEntity blocoDir = AnimatedEntity.CreateRectangle(Game, nameof(blocoDir), new Point(200, 200), Color.DarkCyan);
            blocoDir.Transform.SetViewPosition(AlignType.Right);
            blocoDir.Transform.Move(new Vector2(-50, 0));

            AnimatedEntity blocoEsq = AnimatedEntity.CreateRectangle(Game, nameof(blocoEsq), new Point(300, 200), Color.DarkKhaki);
            blocoEsq.Transform.SetViewPosition(AlignType.LeftBottom);
            blocoEsq.Transform.Move(new Vector2(100, 0));
            blocoEsq.Transform.Rotation = MathHelper.ToRadians(35);

            //Aqui adicionamos as entidades manualmente na tela.
            base.AddEntity(shadow, entity, txtEntity, blocoDir, blocoEsq);
            base.Load();
        }

        private void Entity_OnUpdate(Entity2D source, GameTime gameTime)
        {
            //Código para movimento e rotação da entidade.

            var input = Manager.Input;

            if (input.Keyboard.IsDown(Keys.Right))
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
            //Escala
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
            //Informos aqui o que acontece quando a tela for resetada.

            entity.Transform.SetViewPosition(AlignType.Center);
            entity.Transform.Rotation = 0;

            base.Reset();
        }
    }
}