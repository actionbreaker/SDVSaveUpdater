using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace SDVSaveUpdater2
{
    class Fonctions
    {
        Data mData;

        public Fonctions()
        {
            mData = new Data();
        }

        public bool RemplaceDraivin(XDocument Name, XDocument Game)
        {

            var leaves =
                from e in Name.Descendants()
                where !e.Elements().Any()
                select e;

            for (int i = 0; i < mData.tablefr.GetLength(0); i++)
            {
                foreach (var leaf in leaves)
                {
                    var value = leaf.Value;
                    if (value == mData.tablefr[i, 0])
                    {
                        value = mData.tablefr[i, 1];
                    }

                    leaf.Value = value;
                }
            }
            var leavess =
                from e in Game.Descendants()
                where !e.Elements().Any()
                select e;

            for (int i = 0; i < mData.tablefr.GetLength(0); i++)
            {
                foreach (var leaff in leavess)
                {
                    var value = leaff.Value;
                    if (value == mData.tablefr[i, 0])
                    {
                        value = mData.tablefr[i, 1];
                    }

                    leaff.Value = value;
                }
            }

            return true;
        }

        public string Analyse(XDocument Name, XDocument Game)
        {
            string result = "Votre save est en anglais";

            var leaves =
                from e in Name.Descendants()
                where !e.Elements().Any()
                select e;
            foreach (var leaf in leaves)    // Parcours save
            {
                var value = leaf.Value;     // Value = nom dans save
                if (value == "Grande Grange")
                {
                    result = "Votre save a besoin d'être corrigé";
                }
                if ((value == "Faux") && (result != "Votre save a besoin d'être corrigé"))
                {
                    result = "Votre save est compatible";
                }
            }

            var leavess =
                from e in Game.Descendants()
                where !e.Elements().Any()
                select e;
            foreach (var leaff in leavess)    // Parcours save
            {
                var value = leaff.Value;     // Value = nom dans save
                if (value == "Grande Grange")
                {
                    result = "Votre save a besoin d'être corrigé";
                }
                if ((value == "Faux") && (result != "Votre save a besoin d'être corrigé"))
                {
                    result = "Votre save est compatible";
                }
            }

            return result;
        }
    }
}
