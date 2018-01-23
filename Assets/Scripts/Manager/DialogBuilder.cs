﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

namespace EasyUiTool
{
    /// <summary>
    /// 对话框构建类
    /// </summary>
    public static class DialogBuilder
    {
        /// <summary>
        /// Dialog对象池
        /// </summary>
        private static Dictionary<UiType, UiBase> panelPool = new Dictionary<UiType, UiBase>();

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static UiBase GetDialog(UiType type)
        {
            if (EasyUiToolManager.Instance.EasyUiRootTransform == null)
            {
                Debug.LogError("未指定EasyUiRootTransform");
                return null;
            }
            if (panelPool.ContainsKey(type))
            {
                UiBase panel = panelPool[type];
                panel.ResetSelf();
                return panel;
            }
            //如果池中不存在，则进行创建
            GameObject panelPrefab = Resources.Load(StringConfig.UiPrefabPath + type.ToString()) as GameObject;
            if (panelPrefab == null)
            {
                Debug.LogError(string.Format("缺少{0}预制体", type.ToString()));
                return null;
            }
            GameObject newGo = Object.Instantiate(panelPrefab, EasyUiToolManager.Instance.EasyUiRootTransform);
            UiBase newPanel = newGo.GetComponent<UiBase>();
            if (newPanel == null)
            {
                Debug.LogError(string.Format("{0}预制体上缺少UiBase组件", type.ToString()));
                return null;
            }
            //保证组件的unity生命周期能够得到执行
            newGo.SetActive(true);
            newGo.SetActive(false);
            panelPool.Add(type, newPanel);
            return newPanel;
        }

        /// <summary>
        /// 清空组件池，并销毁所有的实例对象
        /// </summary>
        public static void ClearPool()
        {
            foreach (UiType type in panelPool.Keys)
            {
                UiBase dialog = panelPool[type];
                Object.Destroy(dialog.gameObject);
            }
            panelPool.Clear();
        }
       
    }
}
