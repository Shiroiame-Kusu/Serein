# Serein For JE - 新时代极简服务器面板

<p align="center">
    <img alt="Serein" src="https://socialify.git.ci/Zaitonn/Serein/image?description=1&descriptionEditable=%E6%9E%81%E7%AE%80%E4%BD%86%E5%A4%9A%E5%8A%9F%E8%83%BD%E7%9A%84%E6%9C%8D%E5%8A%A1%E5%99%A8%E9%9D%A2%E6%9D%BF%E8%BD%AF%E4%BB%B6&font=KoHo&logo=https%3A%2F%2Fserein.cc%2Fimg%2FSerein.png&name=1&owner=1&pattern=Circuit%20Board#light">
    <br>
    <img alt="GitHub Stars" src="https://img.shields.io/github/stars/Shiroiame-Kusu/Serein?color=blue">
    <a href="https://github.com/Shiroiame-Kusu/Serein/releases/latest">
        <img src="https://img.shields.io/github/v/release/Shiroiame-Kusu/Serein?color=blue">
    </a>
    <a href="https://github.com/Shiroiame-Kusu/Serein/releases/latest">
        <img alt="GitHub all releases" src="https://img.shields.io/github/downloads/Shiroiame-Kusu/Serein/total?color=blue">
    </a>
    <a href="https://github.com/Shiroiame-Kusu/Serein/actions/workflows/Build.yml">
    <img alt="GitHub bulid" src="https://img.shields.io/github/actions/workflow/status/Shiroiame-Kusu/Serein/Build.yml?branch=main&color=blue">
    </a>
    <a href="https://github.com/Shiroiame-Kusu/Serein">
        <img alt="GitHub repo file count" src="https://img.shields.io/github/languages/code-size/Shiroiame-Kusu/Serein">
    </a>
    <a href="https://app.codacy.com/gh/Shiroiame-Kusu/Serein/">
        <img alt="Codacy Grade" src="https://img.shields.io/codacy/grade/982069cd172d4ef4a40aa4bce4977542?color=blue&logo=Codacy">
    </a>
</p>

- 一个基于`.NET 6`和`NET Framework 4.7.2`的新时代我的世界极简服务器面板
- **官网：<https://serein.cc/>**
- **下载最新版：[Releases](https://github.com/Shiroiame-Kusu/Serein/releases/latest)**

## 建议运行环境

Win7或WinServer2012以上

> 更低的系统版本不保证能稳定运行，必要时务必备份数据

## 使用的Package包

详见 <https://github.com/Shiroiame-Kusu/Serein/network/dependencies>

## 编译

NET SDK >= 6.0

```
dotnet restore
dotnet publish "Serein/WPF/Serein-WPF.csproj" -f net6.0-windows --no-self-contained -p:PublishSingleFile=true -p:RuntimeIdentifier=win-x64 -p:IncludeContentInSingleFile=true
```

## 关于

详见 [关于 - Serein](https://serein.cc/docs/more/about)

## 使用协议

[使用协议 - Serein](https://serein.cc/docs/more/agreement)

---

![Alt](https://repobeats.axiom.co/api/embed/7a3460be03dc55945411aa4d390d740de9a146fe.svg "Repobeats analytics image")
