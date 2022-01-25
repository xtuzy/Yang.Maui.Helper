#if __IOS__ 
using System;
using System.Collections.Generic;
using UIKit;
using View = UIKit.UIView;

namespace Xamarin.Helper.Layouts
{
    public static class NSLayoutConstraintHelper
    {

        public static View AddSubviews(this View view,params View[] subviews)
        {
            foreach(var subview in subviews)
            {
                view.AddSubview(subview);
            }
            return view;
        }

        /// <summary>
        /// Sets the priority(优先级).
        /// See <see cref="NSLayoutConstraint.Priority"/><br/>
        /// The priority of the constraint. Must be in range [0, UILayoutPriority.Required].
        /// </summary>
        /// <param name="constraint">The constraint.</param>
        /// <param name="priority">The priority.</param>
        /// <returns></returns>
        public static NSLayoutConstraint SetPriority(this NSLayoutConstraint constraint, float priority)
        {
            constraint.Priority = priority;
            return constraint;
        }

        /// <summary>
        /// 设置Active必然设置true，因为不设置就不会激活
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="thisView"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static View SetActive(this NSLayoutConstraint constraint, bool active = true)
        {
            constraint.Active = active;
            return constraint.FirstItem as View;
        }

        /// <summary>
        /// Sets the multiplier.
        /// 有问题,慎用!!!.注意这个方法,因为Anchor约束里不能直接修改Multiplier,所以重新创建了一个约束.
        /// See <see cref="https://stackoverflow.com/questions/19593641/can-i-change-multiplier-property-for-nslayoutconstraint/56574862#56574862"/>
        /// </summary>
        /// <param name="constraint">The constraint.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns></returns>
        public static NSLayoutConstraint SetMultiplier(this NSLayoutConstraint constraint, nfloat multiplier)
        {
            NSLayoutConstraint.DeactivateConstraints(new NSLayoutConstraint[] { constraint });
            var newConstraint = NSLayoutConstraint.Create(
                constraint.FirstItem,
                constraint.FirstAttribute,
                constraint.Relation,
                constraint.SecondItem,
                constraint.SecondAttribute,
                multiplier,
                constraint.Constant);
            newConstraint.Priority = constraint.Priority;
            newConstraint.ShouldBeArchived = constraint.ShouldBeArchived;
            newConstraint.SetIdentifier(constraint.GetIdentifier());
            var view = constraint.FirstItem as View;
            //NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[] { newConstraint });
            return newConstraint;
        }

        /// <summary>
        /// Sets the constant.
        /// View1.attribute1 = multiplier × View2.attribute2 + constant.
        /// </summary>
        /// <param name="constraint">The constraint.</param>
        /// <param name="constant">The constant.</param>
        /// <returns></returns>
        public static NSLayoutConstraint SetConstant(this NSLayoutConstraint constraint, nfloat constant)
        {
            constraint.Constant = constant;
            return constraint;
        }

        /// <summary>
        /// 直接设置相对于父View四边的位置
        /// 参考<see cref="https://github.com/roberthein/TinyConstraints"/>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="superView"></param>
        /// <param name="leftConstant"></param>
        /// <param name="topConstant"></param>
        /// <param name="rightConstant"></param>
        /// <param name="bottomConstant"></param>
        public static View EdgesToSuperView(this View view, View superView, int leftConstant = 0, int topConstant = 0, int rightConstant = 0, int bottomConstant = 0)
        {
            view.LeadingAnchor.ConstraintEqualTo(superView.LeadingAnchor, leftConstant).Active = true;
            view.TopAnchor.ConstraintEqualTo(superView.TopAnchor, topConstant).Active = true;
            view.TrailingAnchor.ConstraintEqualTo(superView.TrailingAnchor, rightConstant).Active = true;
            view.BottomAnchor.ConstraintEqualTo(superView.BottomAnchor, bottomConstant).Active = true;
            return view;
        }

        public static View CenterXTo(this View view, View secondView, int constant = 0)
        {
            view.CenterXAnchor.ConstraintEqualTo(secondView.CenterXAnchor, constant).Active = true;
            return view;
        }
        public static View CenterYTo(this View view, View secondView, int constant = 0)
        {
            view.CenterYAnchor.ConstraintEqualTo(secondView.CenterYAnchor, constant).Active = true;
            return view;
        }

        public static View CenterTo(this View view, View secondView, int constant = 0)
        {
            view.CenterXAnchor.ConstraintEqualTo(secondView.CenterXAnchor, constant).Active = true;
            view.CenterYAnchor.ConstraintEqualTo(secondView.CenterYAnchor, constant).Active = true;
            return view;
        }

        public static View LeftToLeft(this View view, View secondView, int constant = 0)
        {
            view.LeadingAnchor.ConstraintEqualTo(secondView.LeadingAnchor, constant).Active = true;
            return view;
        }

        public static View LeftToCenter(this View view, View secondView, int constant = 0)
        {
            view.LeadingAnchor.ConstraintEqualTo(secondView.CenterXAnchor, constant).Active = true;
            return view;
        }

        public static View LeftToRight(this View view, View secondView, int constant = 0)
        {
            view.LeadingAnchor.ConstraintEqualTo(secondView.TrailingAnchor, constant).Active = true;
            return view;
        }

        public static View RightToLeft(this View view, View secondView, int constant = 0)
        {
            view.TrailingAnchor.ConstraintEqualTo(secondView.LeadingAnchor, constant).Active = true;
            return view;
        }
        public static View RightToCenter(this View view, View secondView, int constant = 0)
        {
            view.TrailingAnchor.ConstraintEqualTo(secondView.CenterXAnchor, constant).Active = true;
            return view;
        }

        public static View RightToRight(this View view, View secondView, int constant = 0)
        {
            view.TrailingAnchor.ConstraintEqualTo(secondView.TrailingAnchor, constant).Active = true;
            return view;
        }



        public static View TopToTop(this View view, View secondView, int constant = 0)
        {
            view.TopAnchor.ConstraintEqualTo(secondView.TopAnchor, constant).Active = true;
            return view;
        }

        public static View TopToCenter(this View view, View secondView, int constant = 0)
        {
            view.TopAnchor.ConstraintEqualTo(secondView.CenterYAnchor, constant).Active = true;
            return view;
        }

        public static View TopToBottom(this View view, View secondView, int constant = 0)
        {
            view.TopAnchor.ConstraintEqualTo(secondView.BottomAnchor, constant).Active = true;
            return view;
        }

        public static View BottomToTop(this View view, View secondView, int constant = 0)
        {
            view.BottomAnchor.ConstraintEqualTo(secondView.TopAnchor, constant).Active = true;
            return view;
        }

        public static View BottomToCenter(this View view, View secondView, int constant = 0)
        {
            view.BottomAnchor.ConstraintEqualTo(secondView.CenterYAnchor, constant).Active = true;
            return view;
        }

        public static View BottomToBottom(this View view, View secondView, int constant = 0)
        {
            view.BottomAnchor.ConstraintEqualTo(secondView.BottomAnchor, constant).Active = true;
            return view;
        }

        public static View BaselineToBaseline(this View view, View secondView, int constant = 0)
        {
            view.FirstBaselineAnchor.ConstraintEqualTo(secondView.FirstBaselineAnchor, constant).Active = true;
            return view;
        }

        public static View WidthEqualTo(this View view, View secondView, int constant = 0, float multiplier = 1f)
        {
            view.WidthAnchor.ConstraintEqualTo(secondView.WidthAnchor, multiplier, constant).SetActive();
            return view;
        }
        public static View WidthEqualTo(this View view, int constant)
        {
            view.WidthAnchor.ConstraintEqualTo(constant).SetActive();
            return view;
        }

        public static View MinWidth(this View view, int constant = 0, View secondView = null, float multiplier = 1f)
        {
            if (secondView == null)
                view.WidthAnchor.ConstraintGreaterThanOrEqualTo(constant);
            else
                view.WidthAnchor.ConstraintGreaterThanOrEqualTo(secondView.WidthAnchor, multiplier, constant);

            return view;
        }

        public static View MaxWidth(this View view, int constant = 0, View secondView = null, float multiplier = 1f)
        {
            if (secondView == null)
                view.WidthAnchor.ConstraintLessThanOrEqualTo(constant);
            else
                view.WidthAnchor.ConstraintLessThanOrEqualTo(secondView.WidthAnchor, multiplier, constant);

            return view;
        }
        public static View HeightEqualTo(this View view, View secondView, int constant = 0, float multiplier = 1f)
        {
            view.HeightAnchor.ConstraintEqualTo(secondView.HeightAnchor, multiplier, constant).SetActive();
            return view;
        }

        public static View HeightEqualTo(this View view, int constant)
        {
            view.HeightAnchor.ConstraintEqualTo(constant).SetActive();
            return view;
        }

        public static View MinHeight(this View view, int constant = 0, View secondView = null, float multiplier = 1f)
        {
            if (secondView == null)
                view.HeightAnchor.ConstraintGreaterThanOrEqualTo(constant);
            else
                view.HeightAnchor.ConstraintGreaterThanOrEqualTo(secondView.HeightAnchor, multiplier, constant);

            return view;
        }
        public static View MaxHeight(this View view, int constant = 0, View secondView = null, float multiplier = 1f)
        {
            if (secondView == null)
                view.HeightAnchor.ConstraintLessThanOrEqualTo(constant);
            else
                view.HeightAnchor.ConstraintLessThanOrEqualTo(secondView.HeightAnchor, multiplier, constant);

            return view;
        }
    }

    /// <summary>
    /// 模仿Android中的ConstraintLayout的ConstrainSet实现简单布局逻辑,用法基本相同<br/>
    /// 步骤:<br/>
    /// 1. var set = new ConstrainSet();<br/>
    /// 2. set.Clone(Page);<br/>
    /// 3. set.AddConnect();<br/>
    /// 4. set.ApplyTo(Page);<br/>
    /// </summary>
    public class ConstrainSet
    {
        List<NSLayoutConstraint> Set = new List<NSLayoutConstraint>();
        public ConstrainSet AddConnect(View view, NSLayoutAttribute firstSide, View secondView, NSLayoutAttribute secondSide, float margin, NSLayoutRelation relation = NSLayoutRelation.Equal)
        {
            var constraint = NSLayoutConstraint.Create(view, firstSide, relation, secondView, secondSide, 1, margin);
            Set.Add(constraint);
            return this;
        }

        public ConstrainSet AddConnect(View view, NSLayoutAttribute firstSide, float margin, NSLayoutRelation relation = NSLayoutRelation.Equal)
        {
            var constraint = NSLayoutConstraint.Create(view, firstSide, relation, 1, margin);
            Set.Add(constraint);
            return this;
        }

        /// <summary>
        /// 需要使用Priority的使用该方法
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public ConstrainSet AddConnect(NSLayoutConstraint constraint)
        {
            Set.Add(constraint);
            return this;
        }
        /// <summary>
        /// 激活约束并应用到View,移除和DeactivateView原有约束
        /// </summary>
        /// <param name="view"></param>
        public void ApplyTo(View view)
        {
            NSLayoutConstraint.DeactivateConstraints(view.Constraints);
            NSLayoutConstraint.ActivateConstraints(Set.ToArray());
            view.RemoveConstraints(view.Constraints);
            view.AddConstraints(Set.ToArray());
        }

        /// <summary>
        /// 复制View的约束
        /// </summary>
        /// <param name="view"></param>
        public void Clone(View view)
        {
            Set.Clear();
            Set.AddRange(view.Constraints);
        }

        /// <summary>
        /// 去掉某一个View的约束
        /// </summary>
        /// <param name="view"></param>
        public void Clear(View view)
        {
            List<NSLayoutConstraint> constraints = new List<NSLayoutConstraint>();
            foreach(var constraint in Set)
            {
                if (constraint.FirstItem == view)
                {
                    if (Set.Contains(constraint))
                        constraints.Add(constraint);
                    else
                        throw new InvalidOperationException($"{Set}不包含{view}中某一项约束");
                }
            }
            
            foreach(var constraint in constraints)
            {
                Set.Remove(constraint);
            }
        }
    }
}
#endif