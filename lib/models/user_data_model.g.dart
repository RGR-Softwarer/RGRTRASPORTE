// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_data_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserDataModel _$UserDataModelFromJson(Map<String, dynamic> json) =>
    UserDataModel(
      name: json['name'] as String,
      fullName: json['fullName'] as String,
      birthData: json['birthData'] as String,
      cpf: json['cpf'] as String,
      email: json['email'] as String,
      phoneNumber: json['phoneNumber'] as String,
    );

Map<String, dynamic> _$UserDataModelToJson(UserDataModel instance) =>
    <String, dynamic>{
      'name': instance.name,
      'fullName': instance.fullName,
      'birthData': instance.birthData,
      'cpf': instance.cpf,
      'email': instance.email,
      'phoneNumber': instance.phoneNumber,
    };
