namespace SLG
{
    public class GlobalEventSet : EventSet<int>
    {
        static GlobalEventSet this_obj;
        static bool IsCreate = false;
        public static GlobalEventSet me
        {
            get
            {
                if (IsCreate == false)
                {
                    CreateInstance();
                }

                return this_obj;
            }
        }

        public static void CreateInstance()
        {
            if (IsCreate == true)
                return;

            IsCreate = true;
            this_obj = new GlobalEventSet();
        }

        public static void ReleaseInstance()
        {
            this_obj = null;
            IsCreate = false;
        }

        public static void FireEvent(eEventType type, EventArgs args)
        {
            me.fireEvent((int)type, args);
        }

        public static void FireEvent(int e1, EventArgs args)
        {
            me.fireEvent(e1, args);
        }

        public static void FireEvent(int e1, int e2, EventArgs args)
        {
            FireEvent(e1, args);
            FireEvent(Util.Union(e1, e2), args);
        }

        public static void FireEvent(eEventType type, UI.PanelID id, EventArgs args)
        {
            FireEvent((int)type, (int)id, args);
        }

        // 事件注册
        public static Connection<int> SubscribeEvent(int e1, Subscriber subscriber, int group = -1)
        {
            return me.subscribeEvent(e1, group, subscriber);
        }

        // 事件注册
        public static Connection<int> SubscribeEvent(eEventType type, Subscriber subscriber, int group = -1)
        {
            return me.subscribeEvent((int)type, group, subscriber);
        }

        public static Connection<int> SubscribeEvent(int e1, int e2, Subscriber subscriber, int group = -1)
        {
            return me.subscribeEvent(Util.Union(e1, e2), group, subscriber);
        }

        public static Connection<int> SubscribeEvent(eEventType type, UI.PanelID id, Subscriber subscriber, int group = -1)
        {
            return SubscribeEvent((int)type, (int)id, subscriber, group);
        }
    }
}