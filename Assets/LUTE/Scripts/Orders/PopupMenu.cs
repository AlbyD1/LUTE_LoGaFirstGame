using MoreMountains.Feedbacks;
using UnityEngine;

[OrderInfo("Menu",
             "Popup Menu",
             "Creates a popup menu icon which displays a menu when clicked; the menu is populated by the orders on this node but only supports specific orders")]
[AddComponentMenu("")]
public class PopupMenu : Order
{
    [Tooltip("Custom icon to display for this menu")]
    [SerializeField] protected Sprite customPopupIcon;
    [Tooltip("A custom popup class to use to display this menu - if one is in the scene it will be used instead")]
    [SerializeField] protected PopupIcon setPopupMenuIcon;
    [Tooltip("A custom Menu display to use to display this popup menu")]
    [SerializeField] protected Popup popupWindow;
    [Tooltip("If true, the popup icon will be displayed, otherwise it will be hidden")]
    [SerializeField] protected bool showIcon = true;
    [Tooltip("The feedback to play when the button is clicked")]
    [SerializeField] protected MMFeedbacks buttonFeedback;
    [Tooltip("If true, the popup menu will be redrawn each time it is opened")]
    [SerializeField] protected bool allowRedraw = false;

    public Popup SetPopupWindow { get { return popupWindow; } set { popupWindow = value; } }

    private bool drawn = false;

    public override void OnEnter()
    {
        if (drawn && !allowRedraw)
        {
            Continue();
            return;
        }

        if (setPopupMenuIcon != null)
        {
            PopupIcon.ActivePopupIcon = setPopupMenuIcon;
        }

        if (SetPopupWindow != null)
        {
            Popup.ActivePopupWindow = SetPopupWindow;
        }

        var popupIcon = PopupIcon.GetPopupIcon();
        if (popupIcon != null && showIcon)
        {
            if (customPopupIcon != null)
            {
                popupIcon.SetIcon(customPopupIcon);
            }
            popupIcon.SetActive(true);
        }
        var popupWindow = Popup.GetPopupWindow();
        if (popupWindow != null)
        {
            var orders = ParentNode.OrderList;
            popupIcon.SetPopupWindow(popupWindow);
            UnityEngine.Events.UnityAction action = () =>
{
    buttonFeedback?.PlayFeedbacks();
    popupWindow.OpenClose();
};
            popupIcon.SetAction(action);
            if (orders.Count > 0)
            {
                popupWindow.SetOrders(orders);
                popupWindow.CreateMenuGUI();
            }
        }
        popupIcon.MoveToNextOption();

        drawn = true;

        Continue();
    }
}