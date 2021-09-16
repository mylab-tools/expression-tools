# MyLab.ExpressionTools
[![NuGet Version and Downloads count](https://buildstats.info/nuget/MyLab.ExpressionTools)](https://www.nuget.org/packages/MyLab.ExpressionTools)

```
Поддерживаемые платформы: .NET Core 3.1+
```
Ознакомьтесь с последними изменениями в [журнале изменений](/changelog.md).

## Обзор

Предосталвяет метод расширения получения значения для выражений `Expression.GetValue<T>()`.

Пример:

```c#
Expression expression;

var result = expression.GetValue<object>();
```

