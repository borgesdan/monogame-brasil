// Danilo Borges Santos, 2020.

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Classe que armazena um SpriteFrame e suas listas de Boxes.
    /// </summary>
    public class BoxCollection
    {
        /// <summary>
        /// Obtém ou define o SpriteFrame da coleção.
        /// </summary>
        public SpriteFrame SpriteFrame { get; set; } = new SpriteFrame();
        /// <summary>
        /// Obtém ou define a lista de CollisionBox de um SpriteFrame.
        /// </summary>
        public List<CollisionBox> CollisionBoxes { get; set; }
        /// <summary>
        /// Obtém ou define a lista de AttackBox de um SpriteFrame.
        /// </summary>
        public List<AttackBox> AttackBoxes { get; set; }
        /// <summary>
        /// Obtém ou define o nome do grupo ao qual pertence essa coleção. 
        /// </summary>
        public string Group { get; set; } = string.Empty;

        /// <summary>
        /// Inicializa uma nova instância de BoxCollection.
        /// </summary>
        /// <param name="group">Define o nome do grupo ao qual pertence essa coleção. </param>
        /// <param name="frame">Define o SpriteFrame da coleção.</param>
        public BoxCollection(string group, SpriteFrame frame) : this(group, frame, null, null)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância de BoxCollection.
        /// </summary>
        /// <param name="group">Define o nome do grupo ao qual pertence essa coleção. </param>
        /// <param name="frame">Define o SpriteFrame da coleção.</param>
        /// <param name="cboxes">Define a lista de CollisionBox de um SpriteFrame (pode ser null).</param>
        /// <param name="atkBoxes">Define a lista de AttackBox de um SpriteFrame (pode ser null).</param>        
        public BoxCollection(string group, SpriteFrame frame, CollisionBox[] cboxes, AttackBox[] atkBoxes)
        {
            SpriteFrame = frame;
            CollisionBoxes = cboxes != null ? new List<CollisionBox>(cboxes) : new List<CollisionBox>();
            AttackBoxes = atkBoxes != null ? new List<AttackBox>(atkBoxes) : new List<AttackBox>();
            Group = group;
        }

        /// <summary>
        /// Inicializa uma nova instância de BoxCollection como cópia de outra instância.
        /// </summary>
        /// <param name="source">A instância a ser copiada.</param>
        public BoxCollection(BoxCollection source)
        {
            this.SpriteFrame = source.SpriteFrame;
            this.Group = source.Group;

            source.CollisionBoxes.ForEach(cb => CollisionBoxes.Add(cb));
            source.AttackBoxes.ForEach(ab => AttackBoxes.Add(ab));
        }
    }
}
