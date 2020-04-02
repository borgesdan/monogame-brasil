
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2.Screens
{
    class Screen5 : Screen
    {
        AnimatedEntity entity1;
        AnimatedEntity entity2;

        public Screen5(ScreenManager manager, string name) : base(manager: manager, name, true)
        {
            //Podemos mudar a cor de fundo, configurando GraphicsDevice.Clear(manager.Active.BackgroundColor);
            //deste modo na classe Game.
            BackgroundColor = Color.LightSeaGreen;
        }

        public override void Load()
        {
            //Criamos uma animação que tem problemas de alinhamento no eixo X.
            //Não iremos corrigir no momento.
            Animation strider = new Animation(Game, 100, nameof(strider));
            
            //Criamos o SpriteSheet com seus frames.
            //Cada frame representa uma imagem da ação do personagem.
            Sprite idle = new Sprite(Game, "strider_idle");           
            
            idle.AddFrame
                (
                new SpriteFrame(0, 0, 212, 180),
                new SpriteFrame(256, 0, 212, 180),
                new SpriteFrame(508, 0, 220, 180),
                new SpriteFrame(766, 0, 222, 180),
                new SpriteFrame(1034, 0, 222, 180),
                new SpriteFrame(1296, 0, 218, 180),
                new SpriteFrame(1554, 0, 208, 180),
                new SpriteFrame(1810, 0, 216, 180),
                new SpriteFrame(2074, 0, 212, 180)
                );
            strider.AddSprites(idle);            

            //Criamos então a entidade com esse defeito.
            entity1 = new AnimatedEntity(Game, "strider");
            //Adicionamos a animação.
            entity1.AddAnimation(strider);
            //Alinhamento na tela.
            entity1.Transform.SetViewPosition(AlignType.Left);
            entity1.Transform.Move(50, 0);

            //Agora vamos criar outra animação com o mesmo sprite sheet,
            Animation strider2 = new Animation(Game, 100, nameof(strider2));
            Sprite idle2 = new Sprite(Game, "strider_idle");            
            //Mas agora informamos que vamos informar melhor o alinhamento.            
            idle2.AddFrame
                (                
                new SpriteFrame(0, 0, 212, 180, new Vector2(12,0)),
                //Vector (14,0) significa que a origem da imagem será a partir da coordenada 0 (do eixo X) + 14
                //new SpriteFrame(256... new Vector2(14...,
                //A informação 256 significa que o frame começa desta coordenada, que é a coordenada 0 da origem
                //A origem então (0 + 14) será no primeiro pixel do pé.
                new SpriteFrame(256, 0, 212, 180, new Vector2(14,0)),
                //Continuamos nos próximos frames.
                new SpriteFrame(508, 0, 220, 180, new Vector2(20,0)),                               
                new SpriteFrame(766, 0, 222, 180, new Vector2(22,0)),
                new SpriteFrame(1034, 0, 222, 180, new Vector2(22,0)),
                new SpriteFrame(1296, 0, 218, 180, new Vector2(18,0)),
                new SpriteFrame(1554, 0, 208, 180, new Vector2(8,0)),
                new SpriteFrame(1810, 0, 216, 180, new Vector2(16,0)),
                new SpriteFrame(2074, 0, 212, 180, new Vector2(12,0))
                //Vemos que a distância do pé para o ponto 0 de cada imagem é diferente, pode ser maior ou menor.                
                );
            //Se todos os frames necessitassem de um mesmo valor de alinhamento poderiamos fazer assim
            //idle2.Frames.ForEach(f => f.OriginCorrection = new Vector2(value, 0));
            strider2.AddSprites(idle2);

            //Criamos a segunda entidade, agora com a animação correta.
            entity2 = new AnimatedEntity(Game, "strider2");
            entity2.AddAnimation(strider2);
            entity2.Transform.SetViewPosition(AlignType.Right);
            entity2.Transform.Move(-50, 0);

            //Aqui são as linhas desenhadas na tela.
            AnimatedEntity line1x = AnimatedEntity.CreateRectangle(Game, nameof(line1x), new Point(2, this.Viewport.Height), Color.White);
            line1x.Transform.X = entity1.Transform.X;

            AnimatedEntity line2x = AnimatedEntity.CreateRectangle(Game, nameof(line2x), new Point(2, this.Viewport.Height), Color.White);
            line2x.Transform.X = entity2.Transform.X;

            AnimatedEntity liney = AnimatedEntity.CreateRectangle(Game, nameof(liney), new Point(this.Viewport.Width, 2), Color.White);
            liney.Transform.Y = entity1.Transform.Y;

            //O texto informativo.
            TextEntity txtEntity = new TextEntity(Game, nameof(txtEntity));
            txtEntity.SetFont("default");
            txtEntity.Text.Append("5: Teste de correção da origem da animação no eixo X.");
            txtEntity.Transform.Color = Color.Black;

            AddEntity(entity1, entity2, line1x, line2x, liney);

            base.Load();
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
            //Informamos aqui o que acontece quando a tela for resetada.   
            entity1.ActiveAnimation.ResetIndex();
            entity2.ActiveAnimation.ResetIndex();

            base.Reset();
        }
    }
}