[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<br />
<div align="center">
  <a href="https://github.com/Rickybarb5/Blazored.Diagrams">
    <img src="images/logo.png" alt="Blazored.Diagrams Logo" width="80" height="80">
  </a>

<h3 align="center">Blazored.Diagrams</h3>

  <p align="center">
    A highly customizable and flexible Blazor component for rendering interactive node-based diagrams, flowcharts, and mind maps.
    <br />
    <br />
    <a href="https://rickybarb5.github.io/Blazored.Diagrams/">View Demo</a>
    ·
    <a href="https://github.com/Rickybarb5/Blazored.Diagrams/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    ·
    <a href="https://github.com/Rickybarb5/Blazored.Diagrams/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</div>



<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About The Project</a></li>
    <li><a href="#getting-started">Getting Started</a></li>
    <li><a href="#features">Key Features</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



## About The Project

**Blazored.Diagrams** is a comprehensive Blazor component library that allows you to create interactive diagrams entirely within your C# Blazor application. It is built to offer maximum flexibility, allowing developers to create custom nodes, links, and implement complex interaction behaviors.

It provides a strong C# API, accessible via the `IDiagramService`, for managing diagram state, elements (Layers, Nodes, Ports, Links, Groups), and user interactions (panning, zooming, selection).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

* [![Blazor][Blazor.img]][Blazor-url]

---

## Getting Started

Follow these simple steps to integrate **Blazored.Diagrams** into your Blazor project.

### Installation

1.  **Install the NuGet Package**
    Add the core package to your Blazor project:
    ```sh
    dotnet add package Blazored.Diagrams
    ```

2.  **Register Services**
    In your `Program.cs` or `Startup.cs`, register the required services:
    ```csharp
    builder.Services.AddBlazoredDiagrams();
    ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Features

The library is designed with a set of core features to support professional diagramming applications:

* **Custom Components**: Use custom Razor components for Nodes, Ports, and Links.
* **Diagram Service**: Manage the entire diagram state via a single, injectable C# service (`IDiagramService`).
* **Behaviors**: Enable/disable features like Panning, Zooming, Dragging Nodes/Groups, Multi-Selection, and Link Creation.
* **Layers and Groups**: Organize complex diagrams using separate layers and composite groups.
* **Virtualization**: Optimized rendering for large diagrams.
* **Events**: Full subscription to lifecycle and interaction events (e.g., `NodeClickedEvent`, `LinkAddedEvent`).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Roadmap

- TBD

See the [open issues](https://github.com/Rickybarb5/Blazored.Diagrams/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

---

## Contact

Ricardo - [@RickyBarb5](https://github.com/RickyBarb5)

Project Link: [https://github.com/Rickybarb5/Blazored.Diagrams](https://github.com/Rickybarb5/Blazored.Diagrams)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

[contributors-shield]: https://img.shields.io/github/contributors/Rickybarb5/Blazored.Diagrams.svg?style=for-the-badge
[contributors-url]: https://github.com/Rickybarb5/Blazored.Diagrams/graphs/contributors

[forks-shield]: https://img.shields.io/github/forks/Rickybarb5/Blazored.Diagrams.svg?style=for-the-badge
[forks-url]: https://github.com/Rickybarb5/Blazored.Diagrams/network/members

[stars-shield]: https://img.shields.io/github/stars/Rickybarb5/Blazored.Diagrams.svg?style=for-the-badge
[stars-url]: https://github.com/Rickybarb5/Blazored.Diagrams/stargazers

[issues-shield]: https://img.shields.io/github/issues/Rickybarb5/Blazored.Diagrams.svg?style=for-the-badge
[issues-url]: https://github.com/Rickybarb5/Blazored.Diagrams/issues

[license-shield]: https://img.shields.io/github/license/Rickybarb5/Blazored.Diagrams.svg?style=for-the-badge
[license-url]: https://github.com/Rickybarb5/Blazored.Diagrams/blob/master/LICENSE.txt

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/othneildrew

[product-screenshot]: images/screenshot.png

[Blazor-url]: https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor
[Blazor.img]: https://www.evertop.pl/wp-content/uploads/2021/01/grafiki_blog_blazor-06-768x242.jpg