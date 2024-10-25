namespace OneProject.Desktop.Theme.Infrastructures;

public static class PropertyHelper
{
    #region Overrider Metadata

    public static void OverrideMetadata<TFor>(this DependencyProperty property)
        => property.OverrideMetadata(typeof(TFor),
            new FrameworkPropertyMetadata(typeof(TFor)));

    #endregion

    #region Register

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(default(TProperty)), validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue), validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(changedCallback), validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options), validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(changedCallback, coerceValueCallback), validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, changedCallback), validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, changedCallback, coerceValueCallback), validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback), validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback),
            validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        bool isAnimationProhibited,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback,
                isAnimationProhibited), validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        bool isAnimationProhibited,
        UpdateSourceTrigger defaultUpdateSourceTrigger,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback,
                isAnimationProhibited, defaultUpdateSourceTrigger), validateValueCallback);

    public static DependencyProperty Register<TProperty, TOwner>(
        string property,
        PropertyMetadata metadata,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.Register(property, typeof(TProperty), typeof(TOwner), metadata);

    #endregion

    #region Register Attached ReadOnly

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(default(TProperty)), validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue), validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(changedCallback), validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options), validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(changedCallback, coerceValueCallback), validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, changedCallback), validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, changedCallback, coerceValueCallback), validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback), validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback),
            validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        bool isAnimationProhibited,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback,
                isAnimationProhibited), validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        bool isAnimationProhibited,
        UpdateSourceTrigger defaultUpdateSourceTrigger,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback,
                isAnimationProhibited, defaultUpdateSourceTrigger), validateValueCallback);

    public static DependencyPropertyKey RegisterReadOnly<TProperty, TOwner>(
        string property,
        PropertyMetadata metadata,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterReadOnly(property, typeof(TProperty), typeof(TOwner), metadata);

    #endregion

    #region Register Attached

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(default(TProperty)), validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue), validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(changedCallback), validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options), validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(changedCallback, coerceValueCallback), validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, changedCallback), validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, changedCallback, coerceValueCallback), validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback), validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback),
            validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        bool isAnimationProhibited,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback,
                isAnimationProhibited), validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        bool isAnimationProhibited,
        UpdateSourceTrigger defaultUpdateSourceTrigger,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback,
                isAnimationProhibited, defaultUpdateSourceTrigger), validateValueCallback);

    public static DependencyProperty RegisterAttached<TProperty, TOwner>(
        string property,
        PropertyMetadata metadata,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttached(property, typeof(TProperty), typeof(TOwner), metadata);

    #endregion

    #region Register Attached ReadOnly

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(default(TProperty)), validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue), validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(changedCallback), validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options), validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(changedCallback, coerceValueCallback), validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, changedCallback), validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, changedCallback, coerceValueCallback), validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback), validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback),
            validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        bool isAnimationProhibited,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback,
                isAnimationProhibited), validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        TProperty defaultValue,
        FrameworkPropertyMetadataOptions options,
        PropertyChangedCallback changedCallback,
        CoerceValueCallback coerceValueCallback,
        bool isAnimationProhibited,
        UpdateSourceTrigger defaultUpdateSourceTrigger,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property,
            typeof(TProperty),
            typeof(TOwner),
            new FrameworkPropertyMetadata(defaultValue, options, changedCallback, coerceValueCallback,
                isAnimationProhibited, defaultUpdateSourceTrigger), validateValueCallback);

    public static DependencyPropertyKey RegisterAttachedReadOnly<TProperty, TOwner>(
        string property,
        PropertyMetadata metadata,
        ValidateValueCallback? validateValueCallback = null)
        => DependencyProperty.RegisterAttachedReadOnly(property, typeof(TProperty), typeof(TOwner), metadata);

    #endregion
}

