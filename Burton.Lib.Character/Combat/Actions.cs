using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Burton.Lib.Characters.Combat
{
    public class CombatActionMethodAttribute : Attribute
    {
    }

    public class CombatActionDelegates
    {
        [CombatActionMethod]
        public static void Attack(CombatAction Action, object Sender)
        {
            Debug.Log("Attack!");
        }

        [CombatActionMethod]
        public static void CastSpell(CombatAction Action, object Sender)
        {
            Debug.Log("Cast Spell!");
        }

        [CombatActionMethod]
        public static void Dash(CombatAction Action, object Sender)
        {
            Debug.Log("Dash");
        }

        [CombatActionMethod]
        public static void Disengage(CombatAction Action, object Sender)
        {
            Debug.Log("Disengage!");
        }

        [CombatActionMethod]
        public static void Dodge(CombatAction Action, object Sender)
        {
            Debug.Log("Dodge!");
        }

        [CombatActionMethod]
        public static void Help(CombatAction Action, object Sender)
        {
            Debug.Log("Help!");
        }

        [CombatActionMethod]
        public static void Hide(CombatAction Action, object Sender)
        {
            Debug.Log("Hide!");
        }

        [CombatActionMethod]
        public static void Ready(CombatAction Action, object Sender)
        {
            Debug.Log("Ready!");
        }

        [CombatActionMethod]
        public static void Search(CombatAction Action, object Sender)
        {
            Debug.Log("Search!");
        }

        [CombatActionMethod]
        public static void UseObject(CombatAction Action, object Sender)
        {
            Debug.Log("UseObject!");
        }
    }

    public class CombatActionManager
    {
        #region Singleton
        private static CombatActionManager _Instance;
        public static CombatActionManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new CombatActionManager();

                return _Instance;
            }
        }
        #endregion

        public List<CombatAction> Actions;

        public CombatActionManager()
        {
            RefreshList();
        }

        public void RefreshList()
        {
            Actions = new List<CombatAction>();
            Actions.Add(new CombatAction("Attack", "Attack"));
            Actions.Add(new CombatAction("Cast a Spell", "CastSpell"));
            Actions.Add(new CombatAction("Dash", "Dash"));
            Actions.Add(new CombatAction("Disengage", "Disengage"));
            Actions.Add(new CombatAction("Dodge", "Dodge"));
            Actions.Add(new CombatAction("Help", "Help"));
            Actions.Add(new CombatAction("Hide", "Hide"));
            Actions.Add(new CombatAction("Ready", "Ready"));
            Actions.Add(new CombatAction("Search", "Search"));
            Actions.Add(new CombatAction("Use an Object", "UseObject"));

            foreach (var Action in Actions)
            {
                Action.BindCombatActionMethod<CombatActionDelegates>(Action.ExecuteDelegateName);
            }
        }
    }

    public class CombatAction : ScriptableObject
    {
        #region Delegation
        private Action<CombatAction, object> ExecuteDelegate = null;
        public string ExecuteDelegateName = string.Empty;
        public MethodInfo ExecuteDelegateInfo = null;
        public void BindCombatActionMethod<T>(string MethodName)
        {
            ExecuteDelegateName = MethodName;

            ExecuteDelegateInfo = typeof(T).Assembly
                .GetTypes()
                .SelectMany(x => x.GetMethods())
                .Where(x => x.GetCustomAttributes(true).OfType<CombatActionMethodAttribute>().Any())
                .Where(x => x.Name == MethodName).SingleOrDefault();

            if (ExecuteDelegateInfo == null)
                return;

            ExecuteDelegate = (Action<CombatAction, object>)Delegate.CreateDelegate(typeof(Action<CombatAction, object>), ExecuteDelegateInfo);
        }
        #endregion

        public string Name;

        public CombatAction(string Name, string MethodName)
        {
            this.Name = Name;
            this.ExecuteDelegateName = MethodName;

            BindCombatActionMethod<CombatActionDelegates>(this.ExecuteDelegateName);
        }

        public void Execute(CombatAction Action, object Sender)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate(Action, Sender);
            }
        }
    }
}
