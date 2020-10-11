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
        public List<CollisionBox> CollisionBoxes { get; set; } = new List<CollisionBox>();
        /// <summary>
        /// Obtém ou define a lista de AttackBox de um SpriteFrame.
        /// </summary>
        public List<AttackBox> AttackBoxes { get; set; } = new List<AttackBox>();
        /// <summary>
        /// Obtém ou define o nome do grupo ao qual pertence essa coleção. 
        /// </summary>
        public string Group { get; set; } = string.Empty;


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
        /// Inicializa uma nova instância de BoxCollection.
        /// </summary>
        /// <param name="group">Define o nome do grupo ao qual pertence essa coleção. </param>
        /// <param name="frame">Define o SpriteFrame da coleção.</param>
        /// <param name="cboxes">Define a lista de CollisionBox de um SpriteFrame (pode ser null).</param>
        /// <param name="atkBoxes">Define a lista de AttackBox de um SpriteFrame (pode ser null).</param>
        public BoxCollection(string group, SpriteFrame frame, List<CollisionBox> cboxes, List<AttackBox> atkBoxes)
        {
            SpriteFrame = frame;
            CollisionBoxes = cboxes ?? new List<CollisionBox>();
            AttackBoxes = atkBoxes ?? new List<AttackBox>();
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

    /// <summary>
    /// Classe que armazena instância de BoxCollection com o seu index único.
    /// </summary>
    public class BoxGroup
    {
        /// <summary>
        /// Obtém o Sprite que esse BoxGroup está associado.
        /// </summary>
        public Sprite Sprite { get; private set; } = null;
        /// <summary>
        /// Obtém ou define a lista com os objetos adicionados.
        /// </summary>
        public List<BoxCollection> Values { get; set; } = new List<BoxCollection>();
        /// <summary>
        /// Obtém a quantidade de itens no dicionário.
        /// </summary>
        public int Count { get => Values.Count; }
        
        /// <summary>
        /// Inicializa uma nova instância de BoxGroup.
        /// </summary>
        public BoxGroup(Sprite sprite) 
        {
            Sprite = sprite;
        }

        /// <summary>
        /// Inicializa uma nova instância de BoxGroup como cópia de outra instância.
        /// </summary>
        /// <param name="source">O objeto BoxGroup a ser copiado.</param>
        public BoxGroup(BoxGroup source)
        {
            Sprite = source.Sprite;

            for(int i = 0; i < source.Values.Count; i++)
            {
                Values.Add(new BoxCollection(source.Values[i]));

                //BoxCollection boxes = source.Values[i];
                //SpriteFrame sf = boxes.SpriteFrame;
                //var clist = new List<CollisionBox>();
                //var alist = new List<AttackBox>();
                //string group = "";

                //source.Values[i].CollisionBoxes.ForEach(cb => clist.Add(cb));
                //source.Values[i].AttackBoxes.ForEach(ab => alist.Add(ab));
                //group = source.Values[i].Group;

                //Values.Add(new BoxCollection(group, sf, clist, alist));
            }            
        }
        
        /// <summary>
        /// Obtém ou define a coleção do index informado na lista.
        /// </summary>
        public BoxCollection this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }

        /// <summary>
        /// Adiciona um novo item BoxGroup com o seu index.
        /// </summary>        
        /// <param name="group">Define o nome do grupo ao qual pertence essa coleção. </param>
        /// <param name="spriteFrame">Define o SpriteFrame da coleção.</param>
        /// <param name="collisionBoxes">Define a lista de CollisionBox de um SpriteFrame (pode ser null).</param>
        /// <param name="attackBoxes">Define a lista de AttackBox de um SpriteFrame (pode ser null).</param>
        public void Add(string group, SpriteFrame spriteFrame, CollisionBox[] collisionBoxes, AttackBox[] attackBoxes)
        {
            Values.Add(new BoxCollection(group, spriteFrame, collisionBoxes, attackBoxes));
        }

        /// <summary>
        /// Adiciona um novo item BoxGroup com o seu index.
        /// </summary>
        /// <param name="boxes">A coleção a ser adicionada.</param>
        public void Add(BoxCollection boxes)
        {
            Values.Add(boxes);
        }

        /// <summary>
        /// Remove um item da lista Value.
        /// </summary>
        /// <param name="index">O index a ser acessado.</param>
        public void Remove(int index)
        {
            Values.RemoveAt(index);
        }
        
        /// <summary>
        /// Obtém o BoxCollection associado ao index.
        /// </summary>
        public BoxCollection GetBoxes(int index) => Values[index];
        /// <summary>
        /// Obtém o SpriteFrame associado ao index.
        /// </summary>
        public SpriteFrame GetSpriteFrame(int index) => Values[index].SpriteFrame;
        /// <summary>
        /// Obtém a lista de CollisionBox associado ao index.
        /// </summary>
        public List<CollisionBox> GetCollisionBoxes(int index) => Values[index].CollisionBoxes;
        /// <summary>
        /// Obtém a lista de Attackbox associado ao index.
        /// </summary>
        public List<AttackBox> GetAttackBoxes(int index) => Values[index].AttackBoxes;

        /// <summary>
        /// Obtém uma lista de BoxCollection buscando por um grupo.
        /// </summary>
        public List<BoxCollection> GetByGroup(string groupName)
        {            
            return Values.FindAll(g => g.Group.Equals(groupName));
        }

        /// <summary>
        /// Limpa a lista.
        /// </summary>
        public void Clear()
        {
            Values.Clear();
        }
    }
}