import 'dart:io';

import 'package:either_dart/either.dart';
import 'package:http/http.dart' as http;

class LoginNetwork {
  Future<Either<String, bool>> postLogin(
      String credential, String password) async {
    // TODO(api): DEPOIS DE PRONTO COLOCAR A URL AQUI
    //String url = "url";

    try {
      // TODO(api): DEPOIS DE PRONTO DESCOMENTAR AQUI  E EXCLUIR AS 2 DE BAIXO
      //var response = await http.post(Uri.parse(url));

      var response = (credential == "teste@gmail.com" && password == "teste123")
          ? http.Response("true", 200)
          : http.Response("Cliente nao encontrado", 404);
      if (response.statusCode == HttpStatus.ok ||
          response.statusCode == HttpStatus.created) {
        return const Right(true);
      } else {
        return Left(response.body);
      }
    } catch (error) {
      return Left(error.toString());
    }
  }
}
