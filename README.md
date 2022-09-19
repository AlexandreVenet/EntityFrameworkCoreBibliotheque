# EntityFrameworkCoreBibliotheque

## Enoncé 

- Se servir de **Entity Framework Core**, du **Repository Pattern** et de **SQLServer**.
- Réaliser une application permettant à un utilisateur d’accéder à bibliothèque numérique. 
- Cette bibliothèque devra permettre la consultation d’une liste d’ouvrages qui possèderont un titre, un descriptif, un ISBN, un nombre de page, une catégorie, un auteur et une maison 
d’édition.
- L’auteur possèdera comme propriété un nom, un prénom, une date de naissance. On doit pouvoir consulter le nombre de ses ouvrages.
- Une maison d’édition possèdera un nom. 
- L’utilisateur de l’application devra pouvoir réaliser un CRUD sur les trois types de données contenues dans l’application (chaque donnée devra posséder son propre *repository*) et se voir offrir un menu de navigation entre les différentes sections de l’application. 
- La suppression d’un auteur ou d’une maison d’édition devra supprimer tous les ouvrages qui pourraient leur être liés.

## Paramètres

Le projet utilise **SQLLocalDB**. 

La chaîne de connexion est stockée dans un fichier **secret**.

L'IHM implémente un **IHM à touches clavier**.

Le plan de la BDD est effectué avec **Looping**.