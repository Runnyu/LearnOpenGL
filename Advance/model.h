#ifndef _MODEL_H_
#define _MODEL_H_

#include "mesh.h"
#include "shader_s.h"

#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>

class Model
{
public:
	/*  函数   */
	Model(char *path)
	{
		loadModel(path);
	}
	void Draw(Shader shader);
	/*  模型数据  */
	vector<Mesh> meshes;
	vector<Texture> textures_loaded;
private:
	string directory;
	/*  函数   */
	void loadModel(string path);
	void processNode(aiNode *node, const aiScene *scene);
	Mesh processMesh(aiMesh *mesh, const aiScene *scene);
	vector<Texture> loadMaterialTextures(aiMaterial *mat, aiTextureType type,
		string typeName);
};


#endif